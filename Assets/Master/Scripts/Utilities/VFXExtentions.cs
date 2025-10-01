using UnityEngine;
using DG.Tweening;

public static class VFXExtensions
{
    public static Sequence DOAlphaFlash(this SpriteRenderer renderer, float minAlpha, float maxAlpha, float duration)
    {
        DOTween.Kill(renderer);

        Color color = renderer.color;
        color.a = minAlpha;
        renderer.color = color;

        float halfDuration = duration / 2f;

        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.ToAlpha(() => renderer.color, x => renderer.color = x, maxAlpha, halfDuration));
        seq.Append(DOTween.ToAlpha(() => renderer.color, x => renderer.color = x, minAlpha, halfDuration));
        seq.SetTarget(renderer);

        return seq;
    }
}