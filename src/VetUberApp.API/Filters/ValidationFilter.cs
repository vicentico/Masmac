using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VetUberApp.API.Filters;

/// <summary>
/// Filtro de acción para validación automática de modelos usando FluentValidation
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Si no hay parámetros, continuar
        if (!context.ActionArguments.Any()) 
        {
            await next();
            return;
        }

        // Obtener el primer argumento que necesita validación
        var parameter = context.ActionArguments
            .FirstOrDefault(p => p.Value?.GetType().Name.EndsWith("Dto") ?? false);

        if (parameter.Value == null)
        {
            await next();
            return;
        }

        // Obtener el tipo de validador correspondiente
        var validatorType = typeof(IValidator<>).MakeGenericType(parameter.Value.GetType());
        
        // Intentar obtener el validador del contenedor DI
        var validator = _serviceProvider.GetService(validatorType) as IValidator;
        
        if (validator == null)
        {
            await next();
            return;
        }

        // Realizar la validación
        var validationContext = new ValidationContext<object>(parameter.Value);
        var validationResult = await validator.ValidateAsync(validationContext);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new
            {
                Message = "Error de validación",
                Errors = errors
            });
            return;
        }

        await next();
    }
}