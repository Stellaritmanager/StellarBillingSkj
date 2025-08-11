namespace StellarBillingSystem.Business
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;


        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Handle the exception
                // Return an appropriate response



                context.Session.SetString("ErrorMessage", ex.Message.ToString()); context.Session.SetString("ScreenName", context.GetEndpoint().DisplayName.ToString());

                context.Response.Redirect("./Error");

            }
        }
    }

}
