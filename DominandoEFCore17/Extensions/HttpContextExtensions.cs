namespace DominandoEFCore17.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetTenantId(this HttpContext context)
        {
            // O tenant irá ser informado na rota. É possivel deixa-lo como uma query string ou no header da requisicao
            var tenant = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];

            return tenant;
        }
    }
}