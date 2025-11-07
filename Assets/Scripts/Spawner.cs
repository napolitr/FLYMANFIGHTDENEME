using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using TMPro;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public enum RingType
    {
        Additive, Multiplier, Reducer
    }

    [Serializable]
    public class Ring
    {
        public RingType ringType;
        public int effect;
    }

    [Serializable]
    public class UpperRing
    {
        public Ring[] insideRings = new Ring[2];
    }

    public static Spawner Instance;
    public static List<GameObject> enemies;
    [SerializeField]
    private GameObject Platform;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private float enemyCount;
    [SerializeField] private UpperRing[] rings;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject finish;
    [SerializeField] private float timeDif = 0.5f;

    private float g, averageTime;
    private float velY, velZ;
    private float xPos, yPos, zPos;

    private int ringCount;
    private LevelDatabase.LevelDefinition activeLevelDefinition;
    private LevelDatabase.EnemyStyle activeEnemyStyle;
    private UpperRing[] fallbackRings;
    private MaterialPropertyBlock enemyPropertyBlock;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        enemies = new List<GameObject>();
        fallbackRings = CloneRings(rings);
    }

    void Start()
    {
        g = -Physics.gravity.y;
        var savedLevel = PlayerPrefs.GetInt("level", 0);
        ApplyLevelDefinition(LevelDatabase.GetLevel(savedLevel));
    }

    public void SpawnObjects(Vector3 velocity)
    {
        velY = velocity.y;
        velZ = velocity.z;
        averageTime = velY / g;
        yPos = playerTransform.position.y + (velY * averageTime) - (0.5f * g * averageTime * averageTime);
        zPos = playerTransform.position.z + velZ * averageTime;

        float finishTime = averageTime * 2;
        float finishZPos = playerTransform.position.z + velZ * finishTime;
        GameObject finishGO = SimplePool.Get(finish, new Vector3(0, 0, finishZPos), Quaternion.identity);

        if (enemyCount <= 0f)
        {
            enemyCount = 1f;
        }

        float angle = 360 / enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            float rotation = angle * i;
            GameObject enemy = SimplePool.Get(EnemyPrefab, finishGO.transform.position, Quaternion.Euler(0f, 180f, 0f));
            enemy.transform.Rotate(0, rotation, 0);
            enemy.transform.Translate(new Vector3(0, 0, -16f));
            ApplyEnemyStyle(enemy);
            enemies.Add(enemy);
        }
        
        float t;
        
        if (ringCount % 2 == 0)
        {
            t = averageTime - timeDif / 2 - timeDif * (ringCount - 2) / 2;
        }
        else
        {
            t = averageTime - timeDif * (ringCount / 2);
        }
        
        for (int i = 0; i < ringCount; i++)
        {
            GameObject currentRing = SimplePool.Get(ringPrefab, CalculateRingPosition(t, i), Quaternion.identity);

            for (int j = 0; j < 2; j++)
            {
                Ring currentInsideRing = rings[i].insideRings[j];
                GameObject currentGO = currentRing.transform.GetChild(j).gameObject;

                DetermineRingType(currentInsideRing, currentGO);
            }
            t += timeDif;
        }
    }

    private void DetermineRingType(Ring insideRing, GameObject currentChildGO)
    {
        TextMeshPro ringText = currentChildGO.GetComponentInChildren<TextMeshPro>();

        switch (insideRing.ringType)
        {
            case RingType.Additive:
                var additiveRing = currentChildGO.AddComponent<AdditiveRing>();
                additiveRing.addition = insideRing.effect;
                if (ringText != null)
                {
                    ringText.text = "+" + insideRing.effect;
                }
                break;
            case RingType.Multiplier:
                var multiplierRing = currentChildGO.AddComponent<MultiplierRing>();
                multiplierRing.multiplier = insideRing.effect;
                if (ringText != null)
                {
                    ringText.text = "x" + insideRing.effect;
                }
                break;
            case RingType.Reducer:
                var reducerRing = currentChildGO.AddComponent<ReducerRing>();
                reducerRing.reductionFactor = insideRing.effect;
                if (ringText != null)
                {
                    ringText.text = "-" + insideRing.effect;
                }
                break;
            default:
                if (ringText != null)
                {
                    ringText.text = insideRing.effect.ToString();
                }
                break;
        }

        if (ringText != null && activeLevelDefinition?.Theme != null)
        {
            ringText.color = activeLevelDefinition.Theme.RingColor;
        }
    }

    Vector3 CalculateRingPosition(float t, int i)
    {
        xPos = (i == 0) ? Random.Range(-10f, 10f) : Mathf.Clamp(xPos + 2f, -20f, 20f);

        yPos = playerTransform.position.y + (velY * t) - (0.5f * g * t * t);
        zPos = playerTransform.position.z + velZ * t;

        return new Vector3(xPos, yPos, zPos);
    }

    private void ApplyLevelDefinition(LevelDatabase.LevelDefinition definition)
    {
        activeLevelDefinition = definition;

        if (definition == null)
        {
            ApplyLegacyTheme();
            rings = fallbackRings ?? rings;
            ringCount = rings != null ? rings.Length : 0;
            activeEnemyStyle = null;
            return;
        }

        enemyCount = Mathf.Max(1, definition.EnemyCount);
        timeDif = Mathf.Max(0.1f, definition.TimeBetweenRings);
        rings = BuildRings(definition.Rings);
        ringCount = rings.Length;
        activeEnemyStyle = definition.Enemy;
        ApplyTheme(definition.Theme, definition.Environment);
    }

    private UpperRing[] BuildRings(LevelDatabase.RingPair[] pairs)
    {
        if (pairs == null || pairs.Length == 0)
        {
            return fallbackRings ?? Array.Empty<UpperRing>();
        }

        var result = new UpperRing[pairs.Length];

        for (int i = 0; i < pairs.Length; i++)
        {
            var ringPair = pairs[i];
            var upperRing = new UpperRing
            {
                insideRings = new Ring[2]
            };

            upperRing.insideRings[0] = new Ring
            {
                ringType = ringPair.Inner.RingType,
                effect = ringPair.Inner.Effect
            };

            upperRing.insideRings[1] = new Ring
            {
                ringType = ringPair.Outer.RingType,
                effect = ringPair.Outer.Effect
            };

            result[i] = upperRing;
        }

        return result;
    }

    private void ApplyTheme(GameManager.Colors theme, LevelDatabase.EnvironmentSettings environment)
    {
        if (theme != null)
        {
            Renderer platformRenderer = Platform != null ? Platform.GetComponent<Renderer>() : null;
            if (platformRenderer != null)
            {
                platformRenderer.sharedMaterial.color = theme.PlatformColor;
            }

            if (ringPrefab != null)
            {
                for (int i = 0; i < ringPrefab.transform.childCount; i++)
                {
                    var ringChild = ringPrefab.transform.GetChild(i);
                    var ringRenderer = ringChild != null ? ringChild.GetComponent<Renderer>() : null;
                    if (ringRenderer != null)
                    {
                        ringRenderer.sharedMaterial.color = theme.RingColor;
                    }

                    if (ringChild != null && ringChild.childCount > 1)
                    {
                        var translucent = ringChild.GetChild(1).GetComponent<Renderer>();
                        if (translucent != null)
                        {
                            translucent.sharedMaterial.color = theme.RingTransColor;
                        }
                    }
                }
            }
        }

        if (environment != null)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = environment.FogColor;
            RenderSettings.ambientLight = environment.AmbientColor;
            RenderSettings.ambientSkyColor = environment.AmbientColor;
            RenderSettings.ambientEquatorColor = environment.AmbientColor * 0.75f;
            RenderSettings.ambientGroundColor = environment.AmbientColor * 0.5f;

            if (Camera.main != null)
            {
                Camera.main.backgroundColor = environment.CameraColor;
            }

            Light sun = RenderSettings.sun;
            if (sun == null)
            {
                var lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
                foreach (var light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        sun = light;
                        break;
                    }
                }
            }

            if (sun != null)
            {
                sun.color = environment.LightColor;
            }
        }
    }

    private void ApplyLegacyTheme()
    {
        if (GameManager.Instance == null || GameManager.Instance.ColorArray == null)
        {
            return;
        }

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < 0 || sceneIndex >= GameManager.Instance.ColorArray.Length)
        {
            return;
        }

        ApplyTheme(GameManager.Instance.ColorArray[sceneIndex], null);
    }

    private UpperRing[] CloneRings(UpperRing[] source)
    {
        if (source == null)
        {
            return null;
        }

        var clone = new UpperRing[source.Length];

        for (int i = 0; i < source.Length; i++)
        {
            var original = source[i];
            if (original == null)
            {
                clone[i] = null;
                continue;
            }

            var copiedUpper = new UpperRing
            {
                insideRings = new Ring[original.insideRings.Length]
            };

            for (int j = 0; j < original.insideRings.Length; j++)
            {
                var originalRing = original.insideRings[j];
                if (originalRing == null)
                {
                    copiedUpper.insideRings[j] = null;
                    continue;
                }

                copiedUpper.insideRings[j] = new Ring
                {
                    ringType = originalRing.ringType,
                    effect = originalRing.effect
                };
            }

            clone[i] = copiedUpper;
        }

        return clone;
    }

    private void ApplyEnemyStyle(GameObject enemy)
    {
        if (enemy == null)
        {
            return;
        }

        var theme = activeLevelDefinition?.Theme;
        Color primary = theme != null ? theme.RingColor : Color.white;
        Color secondary = theme != null ? theme.RingTransColor : Color.white;
        float scaleMultiplier = 1f;

        if (activeEnemyStyle != null)
        {
            primary = activeEnemyStyle.PrimaryColor;
            secondary = activeEnemyStyle.SecondaryColor;
            scaleMultiplier = Mathf.Max(0.5f, activeEnemyStyle.ScaleMultiplier);
        }

        enemy.transform.localScale = enemy.transform.localScale * scaleMultiplier;

        if (enemyPropertyBlock == null)
        {
            enemyPropertyBlock = new MaterialPropertyBlock();
        }

        var renderers = enemy.GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in renderers)
        {
            if (renderer == null)
            {
                continue;
            }

            enemyPropertyBlock.Clear();
            renderer.GetPropertyBlock(enemyPropertyBlock);
            enemyPropertyBlock.SetColor("_Color", primary);
            enemyPropertyBlock.SetColor("_BaseColor", primary);
            enemyPropertyBlock.SetColor("_EmissionColor", secondary * 0.8f);
            renderer.SetPropertyBlock(enemyPropertyBlock);
        }

        var particles = enemy.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var particleSystem in particles)
        {
            if (particleSystem == null)
            {
                continue;
            }

            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(primary, secondary);
        }
    }
}
