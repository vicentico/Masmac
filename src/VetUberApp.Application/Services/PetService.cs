using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Application.Services;

public class PetService : IPetService
{
    private readonly IPetRepository _petRepository;
    private readonly IUserRepository _userRepository;

    public PetService(IPetRepository petRepository, IUserRepository userRepository)
    {
        _petRepository = petRepository;
        _userRepository = userRepository;
    }

    public async Task<PetDto> CreateAsync(CreatePetDto dto)
    {
        var owner = await _userRepository.GetByIdAsync(dto.OwnerId)
            ?? throw new InvalidOperationException("Owner not found");

        var pet = new Pet
        {
            Name = dto.Name,
            Type = dto.Type,
            Breed = dto.Breed,
            DateOfBirth = dto.DateOfBirth,
            PhotoUrl = dto.PhotoUrl,
            Weight = dto.Weight,
            SpecialNotes = dto.SpecialNotes,
            OwnerId = dto.OwnerId,
            Owner = owner,
            MedicalHistory = new List<Appointment>()
        };

        await _petRepository.CreateAsync(pet);

        return await MapToDtoAsync(pet);
    }

    public async Task<PetDto?> GetByIdAsync(string id)
    {
        var pet = await _petRepository.GetByIdAsync(id);
        return pet != null ? await MapToDtoAsync(pet) : null;
    }

    public async Task<IEnumerable<PetDto>> GetAllAsync()
    {
        var pets = await _petRepository.GetAllAsync();
        return await Task.WhenAll(pets.Select(MapToDtoAsync));
    }

    public async Task<IEnumerable<PetDto>> GetByUserIdAsync(string userId)
    {
        var pets = await _petRepository.GetByOwnerIdAsync(userId);
        return await Task.WhenAll(pets.Select(MapToDtoAsync));
    }

    public async Task<PetDto> UpdateAsync(string id, UpdatePetDto dto)
    {
        var pet = await _petRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Pet not found");

        pet.Name = dto.Name;
        pet.Breed = dto.Breed;
        pet.PhotoUrl = dto.PhotoUrl;
        pet.Weight = dto.Weight;
        pet.SpecialNotes = dto.SpecialNotes;

        await _petRepository.UpdateAsync(pet);

        return await MapToDtoAsync(pet);
    }

    public async Task DeleteAsync(string id)
    {
        await _petRepository.DeleteAsync(id);
    }

    private async Task<PetDto> MapToDtoAsync(Pet pet)
    {
        var owner = await _userRepository.GetByIdAsync(pet.OwnerId)
            ?? throw new InvalidOperationException("Owner not found");

        return new PetDto(
            pet.Id,
            pet.Name,
            pet.Type,
            pet.Breed,
            pet.DateOfBirth,
            pet.PhotoUrl,
            pet.Weight,
            pet.SpecialNotes,
            pet.OwnerId,
            new UserBasicDto
            {
                Id = owner.Id,
                Name = $"{owner.FirstName} {owner.LastName}",
                Email = owner.Email,
                Phone = owner.PhoneNumber
            });
    }

    public static PetBasicDto MapToBasicDto(Pet pet)
    {
        return new PetBasicDto
        {
            Id = pet.Id,
            Name = pet.Name,
            Type = pet.Type,
            Breed = pet.Breed
        };
    }
}