using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;
using VetUberApp.Application.Factories;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Domain.Constants;

namespace VetUberApp.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IVeterinarianRepository _veterinarianRepository;
    private readonly IPetRepository _petRepository;

    public ReviewService(
        IReviewRepository reviewRepository,
        IAppointmentRepository appointmentRepository,
        IUserRepository userRepository,
        IVeterinarianRepository veterinarianRepository,
        IPetRepository petRepository)
    {
        _reviewRepository = reviewRepository;
        _appointmentRepository = appointmentRepository;
        _userRepository = userRepository;
        _veterinarianRepository = veterinarianRepository;
        _petRepository = petRepository;
    }

    public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
    {
        // Nota: La validación del DTO se maneja automáticamente por FluentValidation en el ValidationFilter
        
        // Verificar que la cita existe
        var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId);
        if (appointment == null)
            throw new InvalidOperationException(ErrorConstants.Appointments.NotFound);

        // Verificar que el usuario es el dueño de la mascota asociada a la cita
        if (appointment.OwnerId != dto.UserId)
            throw new InvalidOperationException("El usuario no está autorizado para crear una reseña para esta cita.");

        // Verificar que el veterinario es el asociado a la cita
        if (appointment.VeterinarianId != dto.VeterinarianId)
            throw new InvalidOperationException("El veterinario especificado no corresponde a esta cita.");

        // Verificar que el usuario no ha creado ya una reseña para esta cita
        var hasReviewed = await _reviewRepository.HasUserReviewedAppointmentAsync(dto.UserId, dto.AppointmentId);
        if (hasReviewed)
            throw new InvalidOperationException(ErrorConstants.Reviews.UserAlreadyReviewed);

        // Validar la calificación
        if (dto.Rating < 1 || dto.Rating > 5)
            throw new InvalidOperationException("La calificación debe estar entre 1 y 5.");

        var review = new Review
        {
            AppointmentId = dto.AppointmentId,
            UserId = dto.UserId,
            VeterinarianId = dto.VeterinarianId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            Type = dto.Type
        };

        await _reviewRepository.CreateAsync(review);

        return await GetReviewDtoAsync(review);
    }

    public async Task<ReviewDto?> GetByIdAsync(string id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
            return null;

        return await GetReviewDtoAsync(review);
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync()
    {
        var reviews = await _reviewRepository.GetAllAsync();
        var dtos = new List<ReviewDto>();

        foreach (var review in reviews)
        {
            dtos.Add(await GetReviewDtoAsync(review));
        }

        return dtos;
    }

    public async Task<IEnumerable<ReviewDto>> GetByVeterinarianIdAsync(string veterinarianId)
    {
        var reviews = await _reviewRepository.GetReviewsByVeterinarianId(veterinarianId);
        var dtos = new List<ReviewDto>();

        foreach (var review in reviews)
        {
            dtos.Add(await GetReviewDtoAsync(review));
        }

        return dtos;
    }

    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(string userId)
    {
        var reviews = await _reviewRepository.GetReviewsByUserId(userId);
        var dtos = new List<ReviewDto>();

        foreach (var review in reviews)
        {
            dtos.Add(await GetReviewDtoAsync(review));
        }

        return dtos;
    }

    public async Task<decimal> GetVeterinarianAverageRatingAsync(string veterinarianId)
    {
        var average = await _reviewRepository.GetAverageRatingByVeterinarianIdAsync(veterinarianId);
        return decimal.Round(average, 2);
    }

    public async Task<ReviewDto> UpdateAsync(string id, UpdateReviewDto dto)
    {
        // Nota: La validación del DTO se maneja automáticamente por FluentValidation en el ValidationFilter
        
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
            throw new InvalidOperationException(ErrorConstants.Reviews.NotFound);

        // Actualizar los campos
        if (dto.Rating.HasValue)
            review.Rating = dto.Rating.Value;
            
        if (!string.IsNullOrEmpty(dto.Comment))
            review.Comment = dto.Comment;

        if (dto.Type.HasValue)
            review.Type = dto.Type.Value;

        await _reviewRepository.UpdateAsync(review);

        return await GetReviewDtoAsync(review);
    }

    public async Task DeleteAsync(string id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
            throw new InvalidOperationException("La reseña especificada no existe.");

        await _reviewRepository.DeleteAsync(id);
        
        // Verify deletion
        var deletedReview = await _reviewRepository.GetByIdAsync(id);
        if (deletedReview != null)
            throw new InvalidOperationException("No se pudo eliminar la reseña.");
    }

    private async Task<ReviewDto> GetReviewDtoAsync(Review review)
    {
        if (string.IsNullOrEmpty(review.Id))
            throw new InvalidOperationException(ErrorConstants.Reviews.InvalidId);

        var appointment = await _appointmentRepository.GetByIdAsync(review.AppointmentId)
            ?? throw new InvalidOperationException(ErrorConstants.Appointments.NotFoundForReview);

        var user = await _userRepository.GetByIdAsync(review.UserId)
            ?? throw new InvalidOperationException(ErrorConstants.Users.NotFoundForReview);

        var veterinarian = await _veterinarianRepository.GetByIdAsync(review.VeterinarianId)
            ?? throw new InvalidOperationException(ErrorConstants.Veterinarians.NotFoundForReview);

        // Obtener la mascota a través de la cita
        var pet = await _petRepository.GetByIdAsync(appointment.PetId)
            ?? throw new InvalidOperationException(ErrorConstants.Pets.NotFoundForAppointment);

        // Asignar entidades de navegación para el Factory
        appointment.Pet = pet;
        appointment.Veterinarian = veterinarian;

        // Usar el Factory para crear el DTO
        return DtoFactory.CreateReviewDto(review, appointment, user, veterinarian);
    }
}