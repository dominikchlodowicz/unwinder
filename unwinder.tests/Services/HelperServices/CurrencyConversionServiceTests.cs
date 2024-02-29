using NUnit.Framework;
using unwinder.Services.HelperServices;
using unwidner.Models.AmadeusApiServiceModels.HotelSearchModels;
using System.Globalization;

namespace unwinder.Tests.Services.HelperServices
{
    [TestFixture]
    public class CurrencyConversionServiceTests
    {
        private CurrencyConversionService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new CurrencyConversionService();
        }

        [Test]
        public void HotelConvertCurrency_WithValidConversion_DictionaryNotNull()
        {
            var hotelSearchOutputData = new HotelSearchOutputModel
            {
                Data = new List<Datum>
                {
                    new Datum
                    {
                        Offers = new List<Offer>
                        {
                            new Offer
                            {
                                Price = new Price
                                {
                                    Currency = "USD",
                                    Total = "100"
                                }
                            }
                        }
                    }
                },
                Dictionaries = new Dictionaries
                {
                    CurrencyConversionLookupRates = new Dictionary<string, CurrencyConversion>
                    {
                        { "USD", new CurrencyConversion { Rate = "1.2", Target = "EUR" } }
                    }
                }
            };

            _service.HotelConvertCurrrency(ref hotelSearchOutputData);

            Assert.That(hotelSearchOutputData.ConvertedCurrencyPrice.CurrencyCode, Is.EqualTo("EUR"));
            Assert.That(hotelSearchOutputData.ConvertedCurrencyPrice.Value, Is.EqualTo(120));
        }

        [Test]
        public void HotelConvertCurrency_DictionaryIsNull()
        {
            var hotelSearchOutputData = new HotelSearchOutputModel
            {
                Data = new List<Datum>
                {
                    new Datum
                    {
                        Offers = new List<Offer>
                        {
                            new Offer
                            {
                                Price = new Price
                                {
                                    Currency = "USD",
                                    Total = "100"
                                }
                            }
                        }
                    }
                },
                Dictionaries = null
            };

            _service.HotelConvertCurrrency(ref hotelSearchOutputData);

            Assert.That(hotelSearchOutputData.ConvertedCurrencyPrice.CurrencyCode, Is.EqualTo("USD"));
            Assert.That(hotelSearchOutputData.ConvertedCurrencyPrice.Value, Is.EqualTo(100));
        }
    }
}
