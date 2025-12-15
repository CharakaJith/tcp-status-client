namespace TcpStatusClient.Device
{
    /// <summary>
    /// provide simulated status for 10 buttons
    /// each button can be either 0 (not pressed) or 1 (pressed)
    /// </summary>
    public static class ButtonStatusProvider
    {
        private static readonly Random _random = new();

        /// <summary>
        /// generate simulated button status
        /// each button can be either 0 (not pressed) or 1 (pressed)
        /// </summary>
        /// <returns>
        /// a string of length 10 representing button statuses (eg: 1001011001)
        /// </returns>
        public static string GetStatus()
        {
            var buttons = new int[10];

            // randomly set each button status
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = _random.Next(0, 2); // generates either 0 or 1
            }

            // convert status array to string and return
            return string.Join("", buttons);
        } 
    }
}