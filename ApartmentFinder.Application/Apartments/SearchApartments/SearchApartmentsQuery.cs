using ApartmentFinder.Application.Abstractions.Messaging;

namespace ApartmentFinder.Application.Apartments.SearchApartments;

public sealed record SearchApartmentsQuery(DateOnly StartDate, DateOnly EndDate) : IQuery<IReadOnlyList<ApartmentResponse>>;
