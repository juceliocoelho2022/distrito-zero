namespace DistritoZero.Player
{
    public sealed class ShotCooldown
    {
        private readonly float interval;
        private float nextAllowedTime = float.NegativeInfinity;

        public ShotCooldown(float interval)
        {
            this.interval = interval < 0f ? 0f : interval;
        }

        public bool TryConsume(float currentTime)
        {
            if (currentTime < nextAllowedTime) return false;
            nextAllowedTime = currentTime + interval;
            return true;
        }
    }
}
