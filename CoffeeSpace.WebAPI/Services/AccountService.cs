using System.Security.Claims;
using AutoMapper;
using CoffeeSpace.Data.Authentication.Response;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.WebAPI.Dto.Requests;
using CoffeeSpace.WebAPI.Services.Interfaces;
using CoffeeSpace.WebAPI.Services.Repository.Interfaces;
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

    public async Task<JwtResponse> LoginAsync(CustomerLoginModel customerViewModel, CancellationToken token = default!)
    {
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(customerViewModel.UserName, customerViewModel.Password, false, false);
        Guard.IsTrue(signInResult.Succeeded, nameof(signInResult));

        Customer? customer = await _signInManager.UserManager.FindByEmailAsync(customerViewModel.Email);
        Guard.IsNotNull(customer);

        customer = await _customerRepo.GetByIdAsync(customer.Id, token);
        
        JwtResponse jwtResponse = new JwtResponse
        {
            Customer = customer,
            Token = await _tokenProvider.GetTokenAsync(customer, token),
            IsSuccess = true
        };

        return jwtResponse;
    }

    public async Task<JwtResponse> RegisterAsync(CustomerRegisterModel customerViewModel, CancellationToken token = default!)
    {
        Customer customer = _mapper.Map<Customer>(customerViewModel);

        IdentityResult identityResult = await _signInManager.UserManager.CreateAsync(customer, customer.Password);
        Guard.IsTrue(identityResult.Succeeded, nameof(identityResult));

        ClaimsPrincipal claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(customer);

        await _signInManager.UserManager.AddClaimsAsync(customer, claimsPrincipal.Claims);
        
        SignInResult signInResult = await _signInManager.PasswordSignInAsync(customer,customer.Password, false, false);
        Guard.IsTrue(signInResult.Succeeded, nameof(signInResult));

        await _customerRepo.AddAsync(customer, token);
        
        JwtResponse jwtResponse = new JwtResponse
        {
            Customer = customer,
            Token = await _tokenProvider.GetTokenAsync(customer, token),
            IsSuccess = true
        };
        
        return jwtResponse;
    }
}