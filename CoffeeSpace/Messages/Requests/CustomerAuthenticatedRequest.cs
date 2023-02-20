﻿using CoffeeSpace.Data.Models.CustomerInfo;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed record CustomerAuthenticatedRequest(Customer Customer) : IRequest<Unit>;