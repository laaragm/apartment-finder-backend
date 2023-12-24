﻿namespace ApartmentFinder.Domain.Shared;

public record Currency
{
    internal static readonly Currency None = new(""); // The internal keyword allows us to hide this field from outside the domain assembly
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Brl = new("BRL");
    public static readonly Currency Eur = new("EUR");
    public static readonly IReadOnlyCollection<Currency> All = new[] { Usd, Brl, Eur };

    public string Code { get; init; }

    private Currency(string code)
    {
        Code = code;
    }

    public static Currency GetByCode(string code)
    {
        return All.FirstOrDefault(x => x.Code == code) ?? throw new ApplicationException("The currency code is invalid");
    }
}
