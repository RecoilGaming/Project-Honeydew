using UnityEngine;

enum AbilityState
{
    READY,
    ACTIVE,
    COOLDOWN
}

public class AbilityHandler : MonoBehaviour
{
    // private variables
    [SerializeField] private Ability ability;
    private AbilityState state;
    private float activeTimer;
    private float cooldownTimer;

    // disable ability
    public void Disable(float dur)
    {
        state = AbilityState.COOLDOWN;
        cooldownTimer = dur;
    }

    // handle ability
    public void Handle(bool inp)
    {
        switch (state)
        {
            case AbilityState.READY:
                if (inp) {
                    ability.Activate(gameObject);
                    state = AbilityState.ACTIVE;
                    activeTimer = ability.activation;
                }
            break;
            case AbilityState.ACTIVE:
                if (activeTimer > 0) {
                    activeTimer -= Time.deltaTime;
                } else {
                    ability.Deactivate(gameObject);
                    state = AbilityState.COOLDOWN;
                    cooldownTimer = ability.cooldown;
                }
            break;
            case AbilityState.COOLDOWN:
                if (cooldownTimer > 0) {
                    cooldownTimer -= Time.deltaTime;
                } else {
                    state = AbilityState.READY;
                }
            break;
        }
    }
}
