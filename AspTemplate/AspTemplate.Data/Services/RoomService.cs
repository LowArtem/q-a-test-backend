using AspTemplate.Core.Dto.Main;
using AspTemplate.Core.Exceptions;
using AspTemplate.Core.Model.Auth;
using AspTemplate.Core.Model.Main;
using AspTemplate.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspTemplate.Data.Services;

public class RoomService
{
    private readonly IEfCoreRepository<Room> _roomRepository;
    private readonly IEfCoreRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public RoomService(IEfCoreRepository<Room> roomRepository, IMapper mapper, IEfCoreRepository<User> userRepository)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Создать комнату
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="userEmail"></param>
    /// <exception cref="EntityNotFoundException"></exception>
    public void CreateRoom(RoomRequestDto requestDto, string userEmail)
    {
        var user = _userRepository.GetListQuery()
            .AsTracking()
            .SingleOrDefault(u => u.Email == userEmail);

        if (user == null)
            throw new EntityNotFoundException(typeof(User));
        
        var mapped = _mapper.Map<Room>(requestDto);
        mapped.User = user;
        
        _roomRepository.Add(mapped);
        _roomRepository.SaveChanges();
    }

    /// <summary>
    /// Получить комнату
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public RoomResponseDto GetRoom(int roomId)
    {
        var room = _roomRepository.Get(roomId);
        if (room == null)
        {
            throw new EntityNotFoundException(typeof(Room));
        }
        
        return _mapper.Map<RoomResponseDto>(room);
    }

    /// <summary>
    /// Получить комнаты
    /// </summary>
    /// <param name="userEmail"></param>
    /// <returns></returns>
    public List<RoomResponseDto> GetRooms(string userEmail)
    {
        return _roomRepository.GetListQuery()
            .Include(r => r.User)
            .Include(r => r.Questions)
            .Where(r => r.User.Email == userEmail)
            .Select(r => _mapper.Map<RoomResponseDto>(r))
            .ToList();
    }

    /// <summary>
    /// Удалить комнату
    /// </summary>
    /// <param name="roomId"></param>
    public void RemoveRoom(int roomId)
    {
        _roomRepository.Remove(roomId);
    }
}