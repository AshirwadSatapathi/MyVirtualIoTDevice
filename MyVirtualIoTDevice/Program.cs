using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace MyVirtualIoTDevice
{
public class Program
{
    private static DeviceClient deviceClient;
    private readonly static string connectionString = "<enter your device connection string>";
    static void Main(string[] args)
    {
        Console.WriteLine("Sending Messages");
        Task.Delay(10000).Wait();
        deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
        int i = 0;
        bool result;
        //sends sensor data from the device simulator every second for 10 minutes
        while (i < 1)
        {
            Task.Delay(6000).Wait();
            result = SendMessages(deviceClient);
            if (result)
            {
                Console.WriteLine($"Message {i} delivered");
            }
            else
            {
                Console.WriteLine("Message failed");
            }
            i++;
        }
    }

    /// <summary>
    /// Method to send data from the device simulator to IoT Hub
    /// </summary>
    /// <param name="deviceClient"></param>
    /// <returns></returns>
    public static bool SendMessages(DeviceClient deviceClient)
    {
        var sensorData = new HealthModel()
        {
            Id = Guid.NewGuid().ToString(),
            PluseRate = GetRandomNumberInRange(90, 110),
            RecordedAt = DateTime.UtcNow,
            Temprature = GetRandomNumberInRange(90, 110)
        };
        var jsonData = JsonConvert.SerializeObject(sensorData);

        try
        {
            var data = new Message(Encoding.ASCII.GetBytes(jsonData));
            deviceClient.SendEventAsync(data).GetAwaiter().GetResult();
            //Console.WriteLine("Message Sent");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Info - {ex.Message}");
            return false;
        }

    }

    /// <summary>
    /// Method to generate random value to mock the Soil Moisture level values
    /// </summary>
    /// <param name="minNumber"></param>
    /// <param name="maxNumber"></param>
    /// <returns></returns>
    public static int GetRandomNumberInRange(int minNumber, int maxNumber)
    {
        return new Random().Next(minNumber,maxNumber);
    }
}
}