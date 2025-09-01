using API.Settings;

var builder = WebApplication.CreateBuilder(args);

await DI.Add(builder);
