using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using FluentValidation;

namespace Application;

public class UserService : IUserService
{
    private IUserRepository _repo;
    private IMapper _mapper;
    private UserDTOValidator _userDtoValidator;

    public UserService(IUserRepository repo,
        IMapper mapper,
        UserDTOValidator userDtoValidator)
    {
        _repo = repo;
        _mapper = mapper;
        _userDtoValidator = userDtoValidator;
    }

    public UserDTO GetUserById(int id)
    {
        if (id < 1) throw new ArgumentException("Id cannot be lower than 1");

        var user = _repo.GetUserById(id);

        if (user == null)
        {
            return null;
        }

        var userDto = new UserDTO()
        {
            Id = user.Id,
            BirthDay = user.BirthDay,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Username = user.Username
        };

        return userDto;
    }

    public UserDTO UpdateUser(UserDTO userDto)
    {
        if (userDto is null) throw new ArgumentException("User is null");
        var user = _mapper.Map<User>(userDto);
        
        var val = _userDtoValidator.Validate(userDto);
        if (!val.IsValid) throw new ArgumentException(val.ToString());

        
        if (_repo.GetUserById(user.Id) is null) {throw new ArgumentException("User id does not exist");}

        return _mapper.Map<UserDTO>(_repo.UpdateUser(user));
    }

    public List<UserDTO> GetAllUsers()
    {
        return _mapper.Map<List<UserDTO>>(_repo.GetAllUsers());
    }
}