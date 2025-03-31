public class ContainerClient(HttpClient httpClient)
{
    // Return the URL of the container.
    public string GetContainerUrl()
    {
        return httpClient.BaseAddress.ToString();
    }
}