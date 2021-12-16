using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Entity
{
    public Player playerOwner;

    public int damage;
    public int sizeEffectRange;
    public RangeType effectRange;
    public AudioClip explosion;
    public bool blockByEntity;

    private Pathfinding pathfinding;
    private Range range;
    private BattleManager battleManager;

    public override void Init()
    {
        pathfinding = GetComponent<Pathfinding>();
        range = GetComponent<Range>();
        battleManager = FindObjectOfType<BattleManager>();
        base.Init();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        List<Tile> r = range.GetRangeTiles(pathfinding.startTile, effectRange, sizeEffectRange, true, blockByEntity);
        foreach (Tile elem in r)
            if (elem.tileEntity != null)
                elem.tileEntity.TakeDamage(damage);
        EndTurn();
    }

    public override void EndTurn()
    {

        base.EndTurn();
        battleManager.FinishTurn();
        Die();
    }

    public override void Die()
    {
        playerOwner.selectedCharacter.RemoveEntityFromAllies(this);
        base.Die();
    }
}
