namespace CoffeeSpace.Services;

public static class PathProvider
{
    public const string GetAll = "/get";
    public const string GetById = "/get/{0}";
    public const string Add = "/add";
    public const string Update = "/update";
    public const string Delete = "/delete/{0}";

    public const string Register = "/auth/register";
    public const string Login = "/auth/login";
}