namespace VetUberApp.Domain.Enums;

public enum UserRole
{
    Client,
    Veterinarian,
    Admin
}

public enum AppointmentStatus
{
    Pending,
    Accepted,
    InProgress,
    Completed,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    Cash
}

public enum PetType
{
    Dog,
    Cat,
    Bird,
    Rabbit,
    Hamster,
    Other
}