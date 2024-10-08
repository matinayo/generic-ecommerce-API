﻿namespace HalceraAPI.Services.Dtos.BaseAddress
{
    public record AddressResponse
    {
        public int Id { get; init; }
        public string? StreetAddress { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Coun   { get; init; }
        public string? PostalCode { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
