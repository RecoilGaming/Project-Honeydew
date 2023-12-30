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
    private float activeTimer;
    private float cooldownTimer;
    private AbilityState state;

    // disable ability
    public void Disable(float dur)
    {
        state = AbilityState.COOLDOWN;
        cooldownTimer = dur;
    }

    // handle ability
    public void Handle(bool inp, PlayerController player)
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
                    cooldownTimer = ability.cooldown - player.attackSpeed * 0.02f;
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
