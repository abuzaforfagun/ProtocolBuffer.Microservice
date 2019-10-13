# ProtocolBuffer.Microservice
Micro service intercommunication using Protocol buffer

## Protocol Buffer
Protocol Buffers is a method of serializing structured data. It is useful in developing programs to communicate with each other over a wire or for storing data. 

Protobuf is used as a short form of Protocol Buffer. It was developed by Google for its internal uses. And In 2008, they released the second version as an open-source.

Because of the performance and lightweight payload, currently, Protobuf is widely using for microservice intercommunication. 

## Protocol Buffer Usage

You need to install the protobuf assembly to use it for serialization. And for each language protobuf has own implementation. You can check out [their official documentation](https://developers.google.com/protocol-buffers/docs/overview).

You also can take a look over [this pluralsight course](https://app.pluralsight.com/library/courses/protocol-buffers-beyond-json-xml) for better understanding.

## Protocol Buffer in .NET

C# is one of the supported languages of protobuf. You can find out the documentation [here](https://developers.google.com/protocol-buffers/docs/csharptutorial). And there are a couple of nuget packages available to deal with protocol buffer in an easy way. Here I am using [protobuf-net](https://www.nuget.org/packages/protobuf-net/) to do the serialization.

## Application Architecture

There are 2 API (Gateway and Weather Service). You can imagine Gateway is the public api and communicating with weather service to provide weather information. To communicate with each other you need to transfer data from one api to another. Basically, we use JSON for this kind of communication. But Protocol Buffer is 10 times faster than JSON serialization.

## Implemanation details

* Install the protobuf-net nuget package in Contracts and Gateway project. 
* Add WebApiContrib.Core.Formatter.Protobuf inside WeatherService project. This package will help us to return data in ```application/x-protobuf``` format.
* Add ```services.AddControllers().AddProtobufFormatters();``` inside the Startup class of WeatherService class.

![image](https://user-images.githubusercontent.com/24603959/66720157-0233fd00-ee1b-11e9-8aee-9442a3e3e489.png)

* Use [ProtoContract] attribute in our contract class and use [ProtoMember(tag)] attributes for each of our property. Please keep in mind that, tag needs to be unique.

![image](https://user-images.githubusercontent.com/24603959/66720167-28f23380-ee1b-11e9-9618-e3285ce8e59e.png)


* Implement the generic http service to deal with http client. You can take a look over the code snippet below:

```
public async Task<T> GetAsync<T>(string url)
{
    var client = new HttpClient();

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
    var response = await client.SendAsync(request);
    return Serializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());
}
```

First thing, I added ```application/x-protobuf``` as Accept value of that http call. And because of ```application/x-protobuf``` we got the data in machine-readable binary format. So we need to convert back to our desired model itself.
This one done though, ```Serializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());```. Note, Serializer is coming from the package protobuf-net.

## Run the application
* Clone this repo.
* Open ```ProtocolBuffer.Microservice.sln``` file.
* Make sure you have selected Gateway and WeatherService both as the startup project.
![Solution Properties](https://user-images.githubusercontent.com/24603959/66720078-da906500-ee19-11e9-8041-9b2406763098.png)
* Check out appsettings of Gateway project.
* Build and run the application.
