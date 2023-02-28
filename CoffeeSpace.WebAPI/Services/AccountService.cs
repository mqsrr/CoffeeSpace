using System.Security.Claims;
using AutoMapper;
using CoffeeSpace.Application.Authentication.Response;
using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Repositories.Interfaces;
using CoffeeSpace.Contracts.Requests.Customer;
using CoffeeSpace.WebAPI.Services.Interfaces;
using CommunityToolkit.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.WebAPI.Services;

public sealed class AccountService : IAccountService
{
    private readonly SignInManager<Customer> _signInManager;
    private readonly IMapper _mapper;
    private readonly ITokenProvider<Customer> _tokenProvider;
    private readonly IRepository<Customer> _customerRepo;
    public AccountService(SignInManager<Customer> signInManager, IMapper mapper, ITokenProvider<Customer> tokenProvider, IRepository<Customer> customerRepo)
    {
        _signInManager = signInManager;
        _mapper = mapper;
        _tokenProvider = tokenProvider;
        _customerRepo = customerRepo;
    }

    public async Task<JwtResponse> LoginAsync(LoginRequest request, CancellationToken token = default!)
    {
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
        Guard.IsTrue(signInResult.Succeeded, nameof(signInResult));

        Customer? customer = await _signInManager.UserManager.FindByEmailAsync(request.Email);
        Guard.IsNotNull(customer);

        customer = await _customerRepo.GetByIdAsync(customer.Id, token);
        
        JwtResponse jwtResponse = new JwtResponse
        {
            Customer = customer!,
            Token = await _tokenProvider.GetTokenAsync(customer!, token),
            IsSuccess = true
        };

        return jwtResponse;
    }

    public async Task<JwtResponse> RegisterAsync(RegisterRequest request, CancellationToken token = default!)
    {
        Customer customer = _mapper.Map<Customer>(request);

        IdentityResult identityResult = await _signInManager.UserManager.CreateAsync(customer, customer.Password);
        Guard.IsTrue(identityResult.Succeeded, nameof(identityResult));

        ClaimsPrincipal claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(customer);

        await _signInManager.UserManager.AddClaimsAsync(customer, claimsPrincipal.Claims);
        
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(customer,customer.Password, false, false);
        Guard.IsTrue(signInResult.Succeeded, nameof(signInResult));

        await _customerRepo.CreateAsync(customer, token);
        
        JwtResponse jwtResponse = new JwtResponse
        {
            Customer = customer,
            Token = await _tokenProvider.GetTokenAsync(customer, token),
            IsSuccess = true
        };
        
        return jwtResponse;
    }
}