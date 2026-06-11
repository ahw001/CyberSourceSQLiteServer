using CybsClass.Cybersource.Models.DTOs;

namespace CybsClass.WebApi.Service.Services.DBOperations;

public static class DbErrorHandler
{
    public static DbErrorResult Build(Exception ex) =>
        new DbErrorResult("ERROR", ex.Message, "Database Error");

    public static DbErrorResult Build(string message, string reason) =>
        new DbErrorResult("ERROR", message, reason);
}
