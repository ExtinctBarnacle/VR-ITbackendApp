namespace VR_ITbackendApp
{
    public class ToDoMiddleware
    {
        //для регистрации сведений о входящих HTTP-запросах (например, URL, метод, заголовки
        public static void RegisterRequest(HttpRequest request) 
        {
            // регистрировать данные в консоли
            Console.WriteLine(request.Path.ToString());
            Console.WriteLine(request.Method.ToString());
            Console.WriteLine(request.Headers.ToString());
        }
    }
}
