
# CoffeeSpace

MAUI client with simple coffee ordering system



## API Overview 

In this section, you will mainly see how the web API was implemented, which features and architecture design were used.

As you might have seen, Coffeespace uses a microservices approach to respond to client requests.

|      API       |    Libraries     |   Services     |
| :------------: |  :-----------:   |  :-----------: |    
|  OrderingAPI   |    Application   | PaymentService |
|  ProductsAPI   |     Messages     | ShipmentService|
|  IdentityAPI   |      Domain      |


**All APIs are using event-driven architecture with the CQRS pattern.
For the message provider, RabbitMQ was chosen as the message provider, with MassTransit as the message library.**

**To create messaging through MassTransit, all message models were moved to the Coffeespace.Messages project.**

Lets start with services, that some microservices can use. 

Service registration is implemented using the Scrutor library, which gives a great opportunity to add your services using reflection, as shown below.

```cs
 public static IServiceCollection AddApplicationService<TInterface>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TInterface>()
            .AddClasses(classes =>
            {
                classes.AssignableTo<TInterface>()
                    .WithoutAttribute<Decorator>();
            })
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));

        return services;
    }
```
Basically, when an interface is parsed to a parameter, it scans all classes from its assembly and implements each of the derived classes as an implementation.
```cs
builder.Services.AddApplicationService(typeof(ICacheService<>));

builder.Services.AddApplicationService<IOrderService>();
``` 

**ICacheService is coming from the Coffeespace.Application class library. There are generic services and settings which can be used across microservices.**

However, you can find that all APIs can have the same libraries and references, but it still doesn't look the same. For example, let's look at the OrderingAPI and ProductsAPI.
```cs
 public Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        return _cache.GetAllOrCreateAsync(CacheKeys.Order.GetAll(buyerId), async () =>
        {
            var orders = await _sender.Send(new GetAllOrdersByBuyerIdQuery
            {
                BuyerId = buyerId
            }, cancellationToken);

            return orders;
        }, cancellationToken);
    }
```
This code comes from IOrderService, which is in the OrderingAPI. As you can see, it's just managing caching and creating queries (or commands, depending on the method). On the other hand, we have IProductService in ProductsAPI.

```cs
    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _sender.Send(new GetAllProductsQuery(), cancellationToken);

        return products;
    }
```
And yes, it's only creating queries and commands. Most of the caching is implemented in IProductRepository.
```cs
public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return _cacheService.GetAllOrCreateAsync(CacheKeys.Products.GetAll, () =>
        {
            var products = _productRepository.GetAllProductsAsync(cancellationToken);
            return products;
        }, cancellationToken);
    }
```

The same caching strategy is here, but it is implemented in CachedProductRepository, which is just a decorator to IProductRepository.

*All APIs are using the same services from Coffeespace.Application, but with different implementations.*


  
### OrderingApi 

This API works as a command center for all messages related to creating, updating, and deleting orders. It publishes events to a message provider that can then be consumed by other microservices. 

#### Libraries
* Masstransit
* Fluent Validation
* Scrutor
* Pomelo (MySql)
* Newtonsoft.Json
* Mediator
* Mapperly


#### Features
* Masstransit Saga StateMachine
* RateLimiter
* Options
* JWT Authentication
* Caching

Here is the workflow of the OrderingApi after an order is submitted.
[![](https://mermaid.ink/img/pako:eNqVVE1vozAQ_StozmxEA4HGh65W2et-SJH2UHFxYdJYAZs1piob5b_vgDGBllSqT8B7897zMPYZMpUjMKjxb4Myw--CP2teptKjVXFtRCYqLo23a2qjStTvkV86Ry3k83vkt1Z5k5kFgLclygVgfxRVNWo5zy8PD86EefvmqRSmf7csXlCdUdlpp-RB6BLzrxbolqsjiSEN83ZHzE62xPv2wkXBn0QhTHut6iQHumMUeIW7NcCzaEMAKz2nT3PYzbscw6v3hxcin6UYk0wZc7hPYuFZEkqXYV3PG30jj-s567vvTTo7XY41s_mpjDi0I0YNKKsCDX7o5_4qsx9vFGFR47hz8rm1-0XhHadRLpa2gnKi0XvYOegcbvzpTziM6m-Ux8lcHMyPVHtF8IHgkoucTuq5A1IwRywxBUaPOdenFFJ5IR5vjNq3MgNmdIM-NFXOjTvVwA6cgvlAZw3YGV6BJas4ieJ1EoVJHEbbTexDCyzarOJgE27iKA7ukvU6vvjwTykSuFuttwHxiRlug_A-2PqANLZK_7AXSX-f9A6PfUEX4_IfFMZduw?type=png)](https://mermaid.live/edit#pako:eNqVVE1vozAQ_StozmxEA4HGh65W2et-SJH2UHFxYdJYAZs1piob5b_vgDGBllSqT8B7897zMPYZMpUjMKjxb4Myw--CP2teptKjVXFtRCYqLo23a2qjStTvkV86Ry3k83vkt1Z5k5kFgLclygVgfxRVNWo5zy8PD86EefvmqRSmf7csXlCdUdlpp-RB6BLzrxbolqsjiSEN83ZHzE62xPv2wkXBn0QhTHut6iQHumMUeIW7NcCzaEMAKz2nT3PYzbscw6v3hxcin6UYk0wZc7hPYuFZEkqXYV3PG30jj-s567vvTTo7XY41s_mpjDi0I0YNKKsCDX7o5_4qsx9vFGFR47hz8rm1-0XhHadRLpa2gnKi0XvYOegcbvzpTziM6m-Ux8lcHMyPVHtF8IHgkoucTuq5A1IwRywxBUaPOdenFFJ5IR5vjNq3MgNmdIM-NFXOjTvVwA6cgvlAZw3YGV6BJas4ieJ1EoVJHEbbTexDCyzarOJgE27iKA7ukvU6vvjwTykSuFuttwHxiRlug_A-2PqANLZK_7AXSX-f9A6PfUEX4_IfFMZduw)

As you can see, the OrderingApi **can both publish and consume messages from other microservices. It uses the Masstransit StateMachine, which provides great opportunities to manage order state.There are five states: Submitted, StockConfirmed, Paid, Shipped, and Canceled.** When an order receives a new state, the OrderStateMachine changes its state in the database. The OrderingApi has two databases: OrderingDb and OrderStateDb. When an order is submitted, it is saved in the OrderingDb, and then the OrderStateMachine sends all the necessary messages. The OrderStateMachine uses the OrderStateDb as a storage for orders. **When an order reaches the Shipped or Canceled state, it is immediately removed from the database.** With this feature, the OrderStateMachine can easily continue to work with messages after it was stopped.

Furthermore, the OrderingApi has one message for the IdentityApi. Basically,** when someone deletes a buyer, it sends a message to the IdentityApi to remove the buyer from its database. It also has a consumer that creates a new buyer if someone completes registration.**

### ProductApi

This has less functionality than the OrderingApi, but is still important because it provides CRUD operations on products and checks that the order is in stock.

#### Libraries
* Masstransit
* Fluent Validation
* Scrutor
* Pomelo (MySql)
* Newtonsoft.Json
* Mediator
* Mapperly

#### Features
* RateLimiter
* Options
* JWT Authentication
* Caching
* Decorator

There is no such difficult logic, it simply implements CRUD operation for products, but still has some features, which I would like to show.

The ProductApi provides CRUD operations for managing products, and it is responsible for verifying the stock of orders.**If the order item's title is not found in the ProductApi database, the product cannot be fulfilled, and the order is moved to the cancel state.**

**ProductApi is designed to be used by clients for product retrieval and creation.** To modify the behavior of the IProductRepository, **the decorator pattern is implemented. With Scrutor, decorators can be added to an existing implementation of interface without altering the original code.** The CachedProductRepository is an example of such a decorator, which implements caching functionality to improve performance.

```cs
internal sealed class ProductRepository : IProductRepository
{
    private readonly IProductDbContext _productDbContext;

    public ProductRepository(IProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var isNotEmpty = await _productDbContext.Products.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Product>();
        }

        return _productDbContext.Products;
    }
```

```cs
[Decorator]
internal sealed class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService<Product> _cacheService;

    public CachedProductRepository(IProductRepository productRepository, ICacheService<Product> cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return _cacheService.GetAllOrCreateAsync(CacheKeys.Products.GetAll, () =>
        {
            var products = _productRepository.GetAllProductsAsync(cancellationToken);
            return products;
        }, cancellationToken);
    }
```

No big changes, just added IProductRepository as an argument for CachedProductRepository. Yeah, no big deal. But if you do this without Scrutor, you will get an exception.

*[Decorator] attribute tells Scrutor to not map this class as implementation for IProductRepository.*

```cs
builder.Services.Decorate<IProductRepository, CachedProductRepository>();
```

Overall, the ProductApi provides useful features, such as rate limiting, options, JWT authentication, and caching, among others.
### IdentityApi

The IdentityApi allows users to log in or register as new users, using IdentityDbContext for authentication and identity storage purposes. 

#### Libraries
* Masstransit
* Fluent Validation
* Scrutorx
* Npgsql (Postgres)
* Newtonsoft.Json
* Mediator
* Mapperly

#### Features
* RateLimiter
* Options
* JWT Authentication
[![](https://mermaid.ink/img/pako:eNqFU1Fr2zAQ_iuHHvbkZvZsx6keCkmcjgwGJSkMil-EdUlFHSmT5LZpyH-fJLfMrbvlHoyl77vT9510R1IrjoQSg79blDWWgm0121USXOyZtqIWeyYtTIEZmDcCpR2CMw9Ob5bwnVl8YochY-4ZS-6yhT146pBShiKtvfekmlmhJHwJG0qLl259o9Wj4KiH2QufXTLLYG2VxiHh2hMWzxa1ZA2sUT-KGk3H677Ti6urGXWQ5Aa0b4h59TpzyJzCSrUWP0CssbDqNkAY2Cjt8K0wVgfFHcnH3NUoXY0AojbQmjcfPkoHL2gnvsOAOzfv868pLKVLZrU18CTs_b8MYWPwo6xGbcVQT6_h2NfkmvDX4Ptb-brqGQTT1jUiR94v3XVyhWavfDOD1B-_buFWPWBPw8wTp2eIwcv_FGyYaM4fn8UZGMtsa8C_-XMiPqWHppCI7FDvmOBubo4eqIgTt8OKUPfLmX6oSCVPgclaq9YHWRNqdYsRaffuVt-mjNANc-Yi4h4ooUfyTOhFOk5G4-QyifN0ksdFlkTkQGhSjMaTIk_SSZp8S7NJPD5F5EUpVyIZpZfjNCniOE_yIiviIiLIhXtHP7vRDhMezrgLCV7I6Q9EIEPk?type=png)](https://mermaid.live/edit#pako:eNqFU1Fr2zAQ_iuHHvbkZvZsx6keCkmcjgwGJSkMil-EdUlFHSmT5LZpyH-fJLfMrbvlHoyl77vT9510R1IrjoQSg79blDWWgm0121USXOyZtqIWeyYtTIEZmDcCpR2CMw9Ob5bwnVl8YochY-4ZS-6yhT146pBShiKtvfekmlmhJHwJG0qLl259o9Wj4KiH2QufXTLLYG2VxiHh2hMWzxa1ZA2sUT-KGk3H677Ti6urGXWQ5Aa0b4h59TpzyJzCSrUWP0CssbDqNkAY2Cjt8K0wVgfFHcnH3NUoXY0AojbQmjcfPkoHL2gnvsOAOzfv868pLKVLZrU18CTs_b8MYWPwo6xGbcVQT6_h2NfkmvDX4Ptb-brqGQTT1jUiR94v3XVyhWavfDOD1B-_buFWPWBPw8wTp2eIwcv_FGyYaM4fn8UZGMtsa8C_-XMiPqWHppCI7FDvmOBubo4eqIgTt8OKUPfLmX6oSCVPgclaq9YHWRNqdYsRaffuVt-mjNANc-Yi4h4ooUfyTOhFOk5G4-QyifN0ksdFlkTkQGhSjMaTIk_SSZp8S7NJPD5F5EUpVyIZpZfjNCniOE_yIiviIiLIhXtHP7vRDhMezrgLCV7I6Q9EIEPk)

**Once a user registers with the IdentityApi, it sends a message to the message provider, and the OrderingApi acts as an external service and receives the message. This process results in the creation of a buyer after registration.**
## Services

In CoffeeSpace, there are two microservices that are referred to as "Services". **These services do not have any controllers of their own and can only be contacted through the message provider.**

#### Services in CoffeeSpace
* PaymentService
* ShipmentService

#### PaymentService
[![](https://mermaid.ink/img/pako:eNp1kk1vwjAMhv9K5dOmFdSWfpEDl3FFQuI29eI1BqLRhKUpWof473NbgUYROVn2-z625ZyhNJJAQE3fDemSlgp3FqtCe_yOaJ0q1RG181ZU17ijtTUnJck-CtbYVqTdhuxJlfS0vkSHn1izYJCMuJPF4u2eJLwNacmoPulVg34w3yv_ea9d2OyMpZtb6Vu4VzWXWo-Fh3vc1Txh4GQ8zLvRW2UrdMpoz2xvuA72bK6OM9pztNUJD0oOzJc-fi00-FARd1KS73PuwAW4PVVUgOBQov0qoNCXXomNM5tWlyCcbciH5si46zVBbPFQc5YPAeIMPyDCNJkmWRqGeZqk8yifZz60nJ7OZ2GcBHkSB9EsjqOLD7_GMCGYZlkUxLMozzIuJnnuA0nFW6-GH9R_pL7FR2_o5rj8AdNDzr4?type=png)](https://mermaid.live/edit#pako:eNp1kk1vwjAMhv9K5dOmFdSWfpEDl3FFQuI29eI1BqLRhKUpWof473NbgUYROVn2-z625ZyhNJJAQE3fDemSlgp3FqtCe_yOaJ0q1RG181ZU17ijtTUnJck-CtbYVqTdhuxJlfS0vkSHn1izYJCMuJPF4u2eJLwNacmoPulVg34w3yv_ea9d2OyMpZtb6Vu4VzWXWo-Fh3vc1Txh4GQ8zLvRW2UrdMpoz2xvuA72bK6OM9pztNUJD0oOzJc-fi00-FARd1KS73PuwAW4PVVUgOBQov0qoNCXXomNM5tWlyCcbciH5si46zVBbPFQc5YPAeIMPyDCNJkmWRqGeZqk8yifZz60nJ7OZ2GcBHkSB9EsjqOLD7_GMCGYZlkUxLMozzIuJnnuA0nFW6-GH9R_pL7FR2_o5rj8AdNDzr4)

**When PaymentService receives a message from the message provider, it adds the payment information from the order to its database (PaymentHistory table). Then, the payment process is initiated and the result of the payment is recorded.**

#### ShipmentService

The ShipmentService is a simple service that returns a successful result. However, in a real-world scenario, it would redirect to an external shipment service to handle the actual shipment.
## Getting started

This project requires docker. **Be assure that you have docker running localy on your machine.** 

It's worth noting that each of the microservices, except for Gateway, requires RabbitMQ credentials, including the host, username, and password. You can use user-secrets as a secret store for these credentials. To initialize user-secrets, you can run the following command:

You can use user-secrets as secrets store.
```sh
dotnet user-secrets init
```

Afterwards, you can add your RabbitMQ credentials using the dotnet user-secrets set command, like so:
```sh
dotnet user-secrets set "RabbitMQ:Host" "<your_RabbitMQ_host>"
dotnet user-secrets set "RabbitMQ:UserName" "<your_RabbitMQ_username>"
dotnet user-secrets set "RabbitMQ:Password" "<your_RabbitMQ_password>"
```
**You can find all the necessary credentials for each microservice in the docker-compose.yaml file.** However, note that this method of providing secrets has its limitations and may not be suitable for all scenarios. In future commits, efforts will be made to streamline the process of providing and managing secrets.

### OrderingAPI
* RabbitMQ
* OrderingDb
* OrderStateDb
* Redis
* JWT Settings

### ProductsAPI
* RabbitMQ
* ProductsDb
* Redis
* JWT Settings

### IdentityAPI
* RabbitMQ
* IdentityDb
* JWT Settings

### PaymentService
* RabbitMQ
* PaymentDb

### ShipmentService
* RabbitMQ

### Gateway
* JWT Settings
## Run Locally

To run CoffeeSpace on your local machine, you need to follow the steps mentioned below:

1. Clone the repository from GitHub by running the following command:

```sh
  git clone https://github.com/Marsik424/CoffeeSpace.git
```
2. Navigate to the "Getting started" section and set up user secrets for each microservice as per the instructions provided there.

3. Open the command prompt or terminal and navigate to the project directory.

4. Run the following command to start the deployment of the microservices to Docker:

```sh
docker-compose up
```

> Note: During the initialization, you may encounter a situation where the OrderingApi and ProductsApi containers are in the "Exited" state. In such cases, wait for approximately 10 seconds for the MySQL databases to initialize.

**Mysql containers are very slow to initialise, to be honest, I tried to fix this but it didn't help. If this problem still exists in a future version of CoffeeSpace, Mysql containers will be removed and replaced with Postgres containers.**

5. After the successful initialization of the containers, you need to manually apply all database migrations by running the following command:

```sh
dotnet ef database update
```

> Note: If the project contains more than one DbContext, use the --context parameter.

**Do not forget to update the server and port in the user-secrets as shown below:**
```json
"ConnectionString: Server:{container-name};Port:{internal-port}; --> ConnectionString: Server:localhost;Port:{external-port};"
```

**All requests should be made to the API Gateway, which will eventually redirect them to the appropriate controller.**

By following these steps, you should be able to run CoffeeSpace locally on your machine.


## FAQ

#### Why is Redis not part of each microservice?

Redis was designed as a high-performance in-memory data structure store. Therefore, there should not be any issues with caching. Additionally, it is easy to create new Redis containers when needed.

#### Can I skip the installation phase if I want to run the project locally?

No, currently, all microservices depend on the secrets store, so you must set up the user-secrets and provide the required credentials for each microservice to run the project successfully. In the future, the plan is to move most of the secrets to the cloud and reduce the setup requirements.

#### Why don't the controllers have external ports in containers?

All requests should be directed to the API Gateway, which will then redirect the request to the appropriate controller. Therefore, there is no need to expose the controller's external ports in the containers. 

#### Why is only the HTTP port available? Will HTTPS be implemented in the future?

Currently, only the HTTP port is available for the CoffeeSpace project. However, in future versions of the project, I plan to implement HTTPS support as well. Stay tuned for updates!
## Contributing

Contributions to CoffeeSpace project are always welcome! If you have any ideas, suggestions or improvements in mind, I would be more than happy to have them! You can start contributing by forking the repository, making your changes and submitting a pull request.

Some areas where you can contribute to are:

* Improving the existing microservices
* Adding new microservices
* Implementing new features
* Refactoring the code
* Improving the documentation
* Adding client support

**Please make sure to follow the code style and the existing patterns in the project.** If you're not sure about something, feel free to create an issue and ask for help.

Also, I recommend discussing your ideas and changes by creating an issue, so I can make sure that your work is in line with the project's goals and direction.

## Related

I was inspired by this project

[eshopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers)

