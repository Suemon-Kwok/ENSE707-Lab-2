using ENSE707_AppointmentBooking;

namespace ENSE707_AppointmentBooking.Tests
{
    [TestClass]
    public class AppointmentBookingServiceTests
    {
        [TestMethod]
    public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsTrue()
    {
        var doctor = new Doctor
        {
            Id = "D001",
            FullName = "Dr Mark",
            AvailableSlots = 2
        };
        var patient = new Patient
        {
            Id = "P001",
            FullName = "Diana William"
        };

        var request = new AppointmentRequest
        {
            Doctor = doctor,
            Patient = patient,
            RequestedDate = DateTime.Today.AddDays(1)
        };

        var service = new AppointmentBookingService();

        bool result = service.BookAppointment(request);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFalse()
    {
        var doctor = new Doctor
        {
            Id = "D001",
            FullName = "Dr Mark",
            AvailableSlots = 0
        };

        var patient = new Patient
        {
            Id = "P001",
            FullName = "Diana William"
        };

        var request = new AppointmentRequest
        {
            Doctor = doctor,
            Patient = patient,
            RequestedDate = DateTime.Today.AddDays(1)
        };

        var service = new AppointmentBookingService();

        bool result = service.BookAppointment(request);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
    {
        var doctor = new Doctor
        {
            Id = "D001",
            FullName = "Dr Mark",
            AvailableSlots = 2
        };

        var patient = new Patient
        {
            Id = "P001",
            FullName = "Diana William"
        };

        var request = new AppointmentRequest
        {
            Doctor = doctor,
            Patient = patient,
            RequestedDate = DateTime.Today.AddDays(1)
        };

        var service = new AppointmentBookingService();

        service.BookAppointment(request);

        Assert.AreEqual(1, doctor.AvailableSlots);
    }

    [TestMethod]
    public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
    {
        var doctor = new Doctor
        {
            Id = "D001",
            FullName = "Dr Mark",
            AvailableSlots = 0
        };
        var patient = new Patient
        {
            Id = "P001",
            FullName = "Diana William"
        };

        var request = new AppointmentRequest
        {
            Doctor = doctor,
            Patient = patient,
            RequestedDate = DateTime.Today.AddDays(1)
        };

        var service = new AppointmentBookingService();

        service.BookAppointment(request);

        Assert.AreEqual(0, doctor.AvailableSlots);
    }
} 

} 
