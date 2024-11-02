using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalBuyPaymentAPI.DTOs;
using PersonalBuyPaymentAPI.Models;
using Stripe;
using Stripe.Checkout;

namespace PersonalBuyPaymentAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeController : ControllerBase
{
    private readonly StripeModel _model;

    public StripeController(IOptions<StripeModel> model)
    {
        _model = model.Value;
    }

    [HttpPost("Payment")]
    public IActionResult Pay([FromBody] List<CartItemDto> cartDto)
    {
        StripeConfiguration.ApiKey = _model.SecretKey;

        var options = new SessionCreateOptions
        {
            LineItems = cartDto.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = Convert.ToInt64(Math.Round(item.Product.Price * 100)),
                    Currency = "brl",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name,
                        Description = item.Product.Description
                    }
                },
                Quantity = item.Quantity
            }).ToList(),
            Mode = "payment",
            SuccessUrl = "http://localhost:4200/success",
            CancelUrl = "http://localhost:4200/"
        };

        var service = new SessionService();

        Session session = service.Create(options);

        return Ok(session.Url);
    }
}
