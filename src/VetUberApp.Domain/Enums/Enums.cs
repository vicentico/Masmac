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

public enum AppointmentType
{
    Regular,
    Emergency,
    Vaccination,
    CheckUp,
    Surgery,
    FollowUp
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
    Cash,
    CreditCard,
    DebitCard,
    BankTransfer,
    MobileWallet,
    Other
}

public enum ReviewType
{
    Overall,
    ProfessionalSkill,
    Communication,
    Punctuality,
    ValueForMoney,
    Cleanliness
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