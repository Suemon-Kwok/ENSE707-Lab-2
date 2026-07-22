namespace ENSE707_AppointmentBooking
{
    public class Doctor
    {
        // Id has no setter - once assigned in the constructor it can never be
        // changed again. This prevents other code from accidentally reassigning
        // a doctor's identity after creation (immutability = reliability).
        public string Id { get; }

        // Same reasoning as Id - a doctor's name shouldn't change after the
        // object is created in this simple model.
        public string FullName { get; }

        // 'private set' means AvailableSlots can be READ from anywhere,
        // but can only be WRITTEN to from inside this class. This stops
        // external code doing "doctor.AvailableSlots = 999" directly and
        // forces all changes to go through ReserveSlot() below, where the
        // rules are enforced (encapsulation).
        public int AvailableSlots { get; private set; }

        // The constructor is the ONLY way to create a Doctor. Because it
        // validates its inputs before assigning anything, it's impossible
        // to end up with a Doctor object that's in an invalid state
        // (e.g. empty name, negative slots).
        public Doctor(string id, string fullName, int availableSlots)
        {
            // Guard clause: reject a missing/blank/whitespace-only ID.
            // IsNullOrWhiteSpace catches null, "", and "   " in one check.
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Doctor ID is required.");

            // Same guard for the doctor's name.
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Doctor name is required.");

            // A doctor can't logically have a negative number of slots -
            // this catches bad data (e.g. from a typo or bug) at the
            // earliest possible point, rather than letting it corrupt
            // later booking logic.
            if (availableSlots < 0)
                throw new ArgumentException("Available slots cannot be negative.");

            // Only reached if all validation passed - the object is
            // guaranteed valid from this point onward.
            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
        }

        // A read-only check other code can call before attempting a
        // booking, without needing to know HOW slots are tracked
        // internally. This keeps the internal int private and hides
        // implementation detail from callers (abstraction).
        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0;
        }

        // The ONLY method allowed to decrease AvailableSlots. Because
        // AvailableSlots has a private setter, this is the single
        // gatekeeper for slot changes - there's exactly one place in
        // the whole codebase where this logic can go wrong, which
        // makes it easy to test and maintain.
        public void ReserveSlot()
        {
            // Defensive check - even though HasAvailableSlot() exists for
            // callers to check first, ReserveSlot() doesn't trust that they
            // did. It re-validates itself so it can never be called into
            // an invalid state, no matter what the caller does.
            if (!HasAvailableSlot())
                throw new InvalidOperationException("No appointment slots are available.");

            AvailableSlots--;
        }
    }
}