using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VetUberApp.API.Filters;

/// <summary>
/// Filtro de acción para validación automática de modelos usando FluentValidation
/// </summary>
/// <remarks>
/// Este filtro intercepta automáticamente todas las acciones del controlador
/// y valida los DTOs usando FluentValidation, devolviendo errores estructurados
/// si la validación falla.
/// </remarks>
public class ValidationFilter : IAsyncActionFilter
{
    #region Constants

    private const string DtoSuffix = "Dto";
    private const string ValidationErrorMessage = "Error de validación";

    #endregion

    #region Fields

    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Constructor

    /// <summary>
    /// Inicializa una nueva instancia del filtro de validación
    /// </summary>
    /// <param name="serviceProvider">Proveedor de servicios para resolver validadores</param>
    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Ejecuta la validación antes de la acción del controlador
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Si no hay parámetros, continuar sin validación
        if (ShouldSkipValidation(context))
        {
            await next();
            return;
        }

        // Obtener el primer DTO que necesita validación
        var dtoParameter = GetFirstDtoParameter(context);
        if (dtoParameter.Value == null)
        {
            await next();
            return;
        }

        // Validar el DTO
        var validationResult = await ValidateParameterAsync(dtoParameter.Value);
        if (!validationResult.IsValid)
        {
            context.Result = CreateValidationErrorResponse(validationResult);
            return;
        }

        await next();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Determina si se debe omitir la validación
    /// </summary>
    private static bool ShouldSkipValidation(ActionExecutingContext context)
    {
        return !context.ActionArguments.Any();
    }

    /// <summary>
    /// Obtiene el primer parámetro que es un DTO
    /// </summary>
    private static KeyValuePair<string, object?> GetFirstDtoParameter(ActionExecutingContext context)
    {
        return context.ActionArguments
            .FirstOrDefault(p => IsDtoParameter(p.Value));
    }

    /// <summary>
    /// Determina si un parámetro es un DTO
    /// </summary>
    private static bool IsDtoParameter(object? parameter)
    {
        return parameter?.GetType().Name.EndsWith(DtoSuffix, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    /// <summary>
    /// Valida un parámetro usando FluentValidation
    /// </summary>
    private async Task<FluentValidation.Results.ValidationResult> ValidateParameterAsync(object parameter)
    {
        var validator = GetValidatorForType(parameter.GetType());
        if (validator == null)
        {
            // Si no hay validador, considerar como válido
            return new FluentValidation.Results.ValidationResult();
        }

        var validationContext = new ValidationContext<object>(parameter);
        return await validator.ValidateAsync(validationContext);
    }

    /// <summary>
    /// Obtiene el validador apropiado para un tipo
    /// </summary>
    private IValidator? GetValidatorForType(Type parameterType)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(parameterType);
        return _serviceProvider.GetService(validatorType) as IValidator;
    }

    /// <summary>
    /// Crea una respuesta de error de validación estructurada
    /// </summary>
    private static BadRequestObjectResult CreateValidationErrorResponse(FluentValidation.Results.ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var errorResponse = new
        {
            Message = ValidationErrorMessage,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };

        return new BadRequestObjectResult(errorResponse);
    }

    #endregion
}