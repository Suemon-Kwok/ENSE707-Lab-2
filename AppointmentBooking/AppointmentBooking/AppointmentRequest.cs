using System;

namespace ENSE707_AppointmentBooking
{
    public class AppointmentRequest
    {
        // Read-only after construction - once a request is created, its
        // contents shouldn't be able to change. This matters because a
        // request represents "what was asked for" - if it could mutate
        // after creation, you'd lose the ability to trust it as a record
        // of the original booking attempt.
        public Patient Patient { get; }
        public Doctor Doctor { get; }
        public DateTime RequestedDate { get; }

        public AppointmentRequest(Patient patient, Doctor doctor, DateTime requestedDate)
        {
            // The ?? (null-coalescing) operator is shorthand for:
            //   if (patient == null) throw new ArgumentNullException(nameof(patient));
            //   Patient = patient;
            // "?? throw" means: use the value on the left if it's not null,
            // otherwise throw the exception on the right. This directly
            // fixes the original quality risk "Patient could be null" -
            // it's now structurally impossible to end up with a request
            // that has no patient attached.
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));

            // Same protection for Doctor - fixes "Doctor could be null".
            // nameof(doctor) passes the string "doctor" into the exception
            // message automatically, so if this ever fails, the error
            // message names the exact broken parameter without it being
            // hardcoded as a string (safer if the parameter is ever renamed).
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));

            // .Date strips the time component off both sides, so this
            // compares calendar dates only (e.g. "23 July" vs "23 July"),
            // not exact timestamps. Without this, a request made at
            // 11:59pm for "today" could incorrectly appear to be "in
            // the past" compared to DateTime.Today's midnight timestamp.
            if (requestedDate.Date < DateTime.Today)
                throw new ArgumentException("Requested appointment date cannot be in the past.");

            // Only reached once patient/doctor are confirmed non-null AND
            // the date is confirmed valid - so by the time this line runs,
            // the object is guaranteed to be in a fully valid state.
            RequestedDate = requestedDate;
        }
    }
}