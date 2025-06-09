using BookService.Controllers;
using BookService.Data;
using BookService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookService.Tests
{
    public class BookingControllerTests
    {
        private readonly Mock<BookingContext> _mockContext;
        private readonly BookingController _controller;

        public BookingControllerTests()
        {
            var options = new DbContextOptions<BookingContext>();
            _mockContext = new Mock<BookingContext>(options);
            _controller = new BookingController(_mockContext.Object);
        }

        [Fact]
        public async Task CreateBooking_ReturnsCreatedAtAction_WhenBookingIsValid()
        {
            // Arrange
            var mockBookings = new Mock<DbSet<Booking>>();
            _mockContext.Setup(m => m.Bookings).Returns(mockBookings.Object);

            var booking = new Booking
            {
                FlightId = 1,
                PassengerId = 1,
                PassengerFirstname = "Anna",
                PassengerLastname = "Schmidt",
                TicketCount = 2
            };
            mockBookings.Setup(m => m.Add(It.IsAny<Booking>())).Callback<Booking>(b => { b.Id = 1; b.CreatedAt = DateTime.UtcNow; b.UpdatedAt = DateTime.UtcNow; });
            _mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var actionResult = await _controller.CreateBooking(booking);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.NotNull(createdAtActionResult);
            Assert.Equal(nameof(BookingController.GetBookingById), createdAtActionResult.ActionName);
            Assert.True(createdAtActionResult.RouteValues!.ContainsKey("id"), "RouteValues should contain 'id'");
            var routeId = createdAtActionResult.RouteValues!["id"];
            Assert.IsType<int>(routeId);
            Assert.Equal(booking.Id, (int)routeId);
            mockBookings.Verify(m => m.Add(It.IsAny<Booking>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
public async Task GetAllBookings_ReturnsOkWithBookings()
{
    // Arrange
    var bookingsList = new List<Booking>
    {
        new Booking { Id = 1, FlightId = 1, PassengerId = 1, PassengerFirstname = "Anna", PassengerLastname = "Schmidt", TicketCount = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        new Booking { Id = 2, FlightId = 2, PassengerId = 2, PassengerFirstname = "Hans", PassengerLastname = "Müller", TicketCount = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
    };
    var mockSet = new Mock<DbSet<Booking>>();
    var queryable = bookingsList.AsQueryable();

    mockSet.As<IQueryable<Booking>>().Setup(m => m.Provider).Returns(queryable.Provider);
    mockSet.As<IQueryable<Booking>>().Setup(m => m.Expression).Returns(queryable.Expression);
    mockSet.As<IQueryable<Booking>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
    mockSet.As<IQueryable<Booking>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

    mockSet.As<IAsyncEnumerable<Booking>>()
        .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
        .Returns(new TestAsyncEnumerator<Booking>(bookingsList.GetEnumerator()));

    _mockContext.Setup(m => m.Bookings).Returns(mockSet.Object);

    // Act
    var actionResult = await _controller.GetAllBookings();

    // Assert
    Assert.NotNull(actionResult);
    var result = actionResult.Result;
    Assert.NotNull(result);
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnBookings = Assert.IsType<List<Booking>>(okResult.Value);
    Assert.Equal(2, returnBookings.Count);
}

        [Fact]
        public async Task GetBookingById_ReturnsNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            var mockBookings = new Mock<DbSet<Booking>>();
            mockBookings.Setup(m => m.FindAsync(1)).ReturnsAsync((Booking?)null);
            _mockContext.Setup(m => m.Bookings).Returns(mockBookings.Object);

            // Act
            var actionResult = await _controller.GetBookingById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetBookingById_ReturnsOkWithBooking_WhenBookingExists()
        {
            // Arrange
            var mockBookings = new Mock<DbSet<Booking>>();
            var booking = new Booking { Id = 1, FlightId = 1, PassengerId = 1, PassengerFirstname = "Anna", PassengerLastname = "Schmidt", TicketCount = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            mockBookings.Setup(m => m.FindAsync(1)).ReturnsAsync(booking);
            _mockContext.Setup(m => m.Bookings).Returns(mockBookings.Object);

            // Act
            var actionResult = await _controller.GetBookingById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnBooking = Assert.IsType<Booking>(okResult.Value);
            Assert.Equal(booking.Id, returnBooking.Id);
        }
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}