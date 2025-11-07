using UnityEngine;

public static class LevelDatabase
{
    public class LevelDefinition
    {
        public GameManager.Colors Theme;
        public EnvironmentSettings Environment;
        public float TimeBetweenRings;
        public int EnemyCount;
        public RingPair[] Rings;
        public EnemyStyle Enemy;
    }

    public class EnvironmentSettings
    {
        public Color FogColor;
        public Color AmbientColor;
        public Color CameraColor;
        public Color LightColor;
    }

    public class EnemyStyle
    {
        public Color PrimaryColor;
        public Color SecondaryColor;
        public float ScaleMultiplier = 1f;
    }

    public struct RingPair
    {
        public RingData Inner;
        public RingData Outer;
    }

    public struct RingData
    {
        public Spawner.RingType RingType;
        public int Effect;
    }

    private static readonly LevelDefinition[] Levels =
    {
        Level(
            theme: Theme(
                new Color(0.98f, 0.43f, 0.40f, 0f),
                new Color(1f, 0.72f, 0.56f, 0.45f),
                new Color(0.99f, 0.83f, 0.72f, 0.65f)
            ),
            env: Env(
                new Color(0.24f, 0.18f, 0.27f, 1f),
                new Color(0.18f, 0.14f, 0.24f, 1f),
                new Color(0.12f, 0.09f, 0.15f, 1f),
                new Color(1f, 0.83f, 0.65f, 1f)
            ),
            enemy: Enemy(
                new Color(1f, 0.64f, 0.52f, 1f),
                new Color(0.98f, 0.43f, 0.40f, 1f),
                1f
            ),
            timeBetweenRings: 2f,
            enemyCount: 10,
            Pair(Spawner.RingType.Additive, 6, Spawner.RingType.Additive, 4),
            Pair(Spawner.RingType.Multiplier, 2, Spawner.RingType.Additive, 5),
            Pair(Spawner.RingType.Reducer, 2, Spawner.RingType.Additive, 5)
        ),
        Level(
            theme: Theme(
                new Color(0.21f, 0.77f, 0.82f, 0f),
                new Color(0.33f, 0.9f, 0.95f, 0.45f),
                new Color(0.24f, 0.68f, 0.74f, 0.6f)
            ),
            env: Env(
                new Color(0.09f, 0.21f, 0.28f, 1f),
                new Color(0.08f, 0.17f, 0.25f, 1f),
                new Color(0.05f, 0.12f, 0.18f, 1f),
                new Color(0.72f, 0.93f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.33f, 0.9f, 0.95f, 1f),
                new Color(0.17f, 0.55f, 0.78f, 1f),
                1f
            ),
            timeBetweenRings: 2f,
            enemyCount: 12,
            Pair(Spawner.RingType.Additive, 4, Spawner.RingType.Additive, 3),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 8),
            Pair(Spawner.RingType.Reducer, 8, Spawner.RingType.Additive, 5)
        ),
        Level(
            theme: Theme(
                new Color(0.78f, 0.42f, 1f, 0f),
                new Color(0.56f, 0.3f, 0.98f, 0.45f),
                new Color(0.59f, 0.36f, 0.92f, 0.6f)
            ),
            env: Env(
                new Color(0.18f, 0.11f, 0.25f, 1f),
                new Color(0.14f, 0.1f, 0.21f, 1f),
                new Color(0.07f, 0.04f, 0.12f, 1f),
                new Color(0.93f, 0.78f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.82f, 0.55f, 1f, 1f),
                new Color(0.63f, 0.28f, 0.98f, 1f),
                1.05f
            ),
            timeBetweenRings: 2.4f,
            enemyCount: 15,
            Pair(Spawner.RingType.Multiplier, 3, Spawner.RingType.Additive, 5),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Reducer, 5),
            Pair(Spawner.RingType.Reducer, 5, Spawner.RingType.Additive, 5),
            Pair(Spawner.RingType.Additive, 10, Spawner.RingType.Reducer, 5)
        ),
        Level(
            theme: Theme(
                new Color(0.99f, 0.81f, 0.27f, 0f),
                new Color(1f, 0.92f, 0.47f, 0.4f),
                new Color(0.98f, 0.87f, 0.43f, 0.6f)
            ),
            env: Env(
                new Color(0.29f, 0.23f, 0.12f, 1f),
                new Color(0.24f, 0.19f, 0.1f, 1f),
                new Color(0.14f, 0.11f, 0.06f, 1f),
                new Color(1f, 0.89f, 0.55f, 1f)
            ),
            enemy: Enemy(
                new Color(1f, 0.88f, 0.43f, 1f),
                new Color(0.99f, 0.63f, 0.2f, 1f),
                1.1f
            ),
            timeBetweenRings: 2.4f,
            enemyCount: 18,
            Pair(Spawner.RingType.Additive, 4, Spawner.RingType.Additive, 6),
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Reducer, 8),
            Pair(Spawner.RingType.Additive, 8, Spawner.RingType.Reducer, 6),
            Pair(Spawner.RingType.Additive, 5, Spawner.RingType.Additive, 8)
        ),
        Level(
            theme: Theme(
                new Color(0.91f, 0.28f, 0.24f, 0f),
                new Color(1f, 0.45f, 0.32f, 0.45f),
                new Color(0.65f, 0.22f, 0.18f, 0.6f)
            ),
            env: Env(
                new Color(0.14f, 0.05f, 0.05f, 1f),
                new Color(0.12f, 0.06f, 0.06f, 1f),
                new Color(0.06f, 0.02f, 0.02f, 1f),
                new Color(1f, 0.58f, 0.36f, 1f)
            ),
            enemy: Enemy(
                new Color(1f, 0.46f, 0.34f, 1f),
                new Color(0.92f, 0.28f, 0.24f, 1f),
                1.18f
            ),
            timeBetweenRings: 2.5f,
            enemyCount: 20,
            Pair(Spawner.RingType.Additive, 8, Spawner.RingType.Additive, 6),
            Pair(Spawner.RingType.Reducer, 4, Spawner.RingType.Additive, 4),
            Pair(Spawner.RingType.Multiplier, 3, Spawner.RingType.Additive, 6),
            Pair(Spawner.RingType.Additive, 4, Spawner.RingType.Additive, 6),
            Pair(Spawner.RingType.Additive, 6, Spawner.RingType.Reducer, 6)
        ),
        Level(
            theme: Theme(
                new Color(0.32f, 0.89f, 0.58f, 0f),
                new Color(0.48f, 0.98f, 0.74f, 0.45f),
                new Color(0.38f, 0.76f, 0.58f, 0.6f)
            ),
            env: Env(
                new Color(0.11f, 0.24f, 0.18f, 1f),
                new Color(0.09f, 0.19f, 0.15f, 1f),
                new Color(0.05f, 0.12f, 0.09f, 1f),
                new Color(0.8f, 1f, 0.88f, 1f)
            ),
            enemy: Enemy(
                new Color(0.53f, 0.98f, 0.74f, 1f),
                new Color(0.23f, 0.68f, 0.48f, 1f),
                1f
            ),
            timeBetweenRings: 2.6f,
            enemyCount: 22,
            Pair(Spawner.RingType.Additive, 6, Spawner.RingType.Additive, 8),
            Pair(Spawner.RingType.Multiplier, 3, Spawner.RingType.Additive, 7),
            Pair(Spawner.RingType.Reducer, 3, Spawner.RingType.Additive, 8),
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Reducer, 2),
            Pair(Spawner.RingType.Additive, 12, Spawner.RingType.Multiplier, 2)
        ),
        Level(
            theme: Theme(
                new Color(0.97f, 0.63f, 0.29f, 0f),
                new Color(0.99f, 0.78f, 0.44f, 0.45f),
                new Color(0.9f, 0.64f, 0.36f, 0.6f)
            ),
            env: Env(
                new Color(0.36f, 0.24f, 0.14f, 1f),
                new Color(0.28f, 0.19f, 0.11f, 1f),
                new Color(0.18f, 0.12f, 0.08f, 1f),
                new Color(1f, 0.86f, 0.53f, 1f)
            ),
            enemy: Enemy(
                new Color(1f, 0.71f, 0.39f, 1f),
                new Color(0.87f, 0.45f, 0.18f, 1f),
                1.08f
            ),
            timeBetweenRings: 2.6f,
            enemyCount: 24,
            Pair(Spawner.RingType.Additive, 7, Spawner.RingType.Multiplier, 3),
            Pair(Spawner.RingType.Additive, 5, Spawner.RingType.Reducer, 4),
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Additive, 10),
            Pair(Spawner.RingType.Reducer, 5, Spawner.RingType.Additive, 9),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 8)
        ),
        Level(
            theme: Theme(
                new Color(0.46f, 0.52f, 0.99f, 0f),
                new Color(0.33f, 0.39f, 0.94f, 0.45f),
                new Color(0.37f, 0.4f, 0.82f, 0.6f)
            ),
            env: Env(
                new Color(0.12f, 0.14f, 0.33f, 1f),
                new Color(0.1f, 0.12f, 0.27f, 1f),
                new Color(0.06f, 0.07f, 0.18f, 1f),
                new Color(0.75f, 0.82f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.51f, 0.57f, 1f, 1f),
                new Color(0.29f, 0.31f, 0.85f, 1f),
                1.12f
            ),
            timeBetweenRings: 2.7f,
            enemyCount: 26,
            Pair(Spawner.RingType.Additive, 8, Spawner.RingType.Reducer, 4),
            Pair(Spawner.RingType.Multiplier, 3, Spawner.RingType.Additive, 9),
            Pair(Spawner.RingType.Additive, 12, Spawner.RingType.Multiplier, 4),
            Pair(Spawner.RingType.Reducer, 6, Spawner.RingType.Additive, 11),
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Multiplier, 3),
            Pair(Spawner.RingType.Additive, 15, Spawner.RingType.Reducer, 5)
        ),
        Level(
            theme: Theme(
                new Color(0.32f, 0.67f, 0.32f, 0f),
                new Color(0.45f, 0.78f, 0.45f, 0.45f),
                new Color(0.34f, 0.56f, 0.37f, 0.6f)
            ),
            env: Env(
                new Color(0.12f, 0.18f, 0.13f, 1f),
                new Color(0.11f, 0.16f, 0.12f, 1f),
                new Color(0.07f, 0.11f, 0.07f, 1f),
                new Color(0.84f, 0.96f, 0.66f, 1f)
            ),
            enemy: Enemy(
                new Color(0.47f, 0.82f, 0.47f, 1f),
                new Color(0.24f, 0.57f, 0.27f, 1f),
                1f
            ),
            timeBetweenRings: 2.7f,
            enemyCount: 28,
            Pair(Spawner.RingType.Multiplier, 3, Spawner.RingType.Additive, 12),
            Pair(Spawner.RingType.Reducer, 5, Spawner.RingType.Additive, 10),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Reducer, 4),
            Pair(Spawner.RingType.Additive, 14, Spawner.RingType.Additive, 9),
            Pair(Spawner.RingType.Reducer, 6, Spawner.RingType.Multiplier, 4),
            Pair(Spawner.RingType.Additive, 16, Spawner.RingType.Multiplier, 3)
        ),
        Level(
            theme: Theme(
                new Color(0.53f, 0.78f, 0.99f, 0f),
                new Color(0.67f, 0.88f, 1f, 0.45f),
                new Color(0.55f, 0.74f, 0.92f, 0.6f)
            ),
            env: Env(
                new Color(0.18f, 0.27f, 0.34f, 1f),
                new Color(0.16f, 0.24f, 0.3f, 1f),
                new Color(0.1f, 0.16f, 0.22f, 1f),
                new Color(0.86f, 0.95f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.63f, 0.88f, 1f, 1f),
                new Color(0.32f, 0.54f, 0.83f, 1f),
                1.05f
            ),
            timeBetweenRings: 2.85f,
            enemyCount: 32,
            Pair(Spawner.RingType.Additive, 11, Spawner.RingType.Additive, 13),
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Reducer, 5),
            Pair(Spawner.RingType.Additive, 16, Spawner.RingType.Multiplier, 5),
            Pair(Spawner.RingType.Reducer, 8, Spawner.RingType.Additive, 14),
            Pair(Spawner.RingType.Multiplier, 6, Spawner.RingType.Additive, 10),
            Pair(Spawner.RingType.Additive, 18, Spawner.RingType.Reducer, 6),
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Additive, 20)
        ),
        Level(
            theme: Theme(
                new Color(0.82f, 0.28f, 0.55f, 0f),
                new Color(0.92f, 0.43f, 0.66f, 0.45f),
                new Color(0.69f, 0.27f, 0.49f, 0.6f)
            ),
            env: Env(
                new Color(0.2f, 0.08f, 0.18f, 1f),
                new Color(0.18f, 0.07f, 0.15f, 1f),
                new Color(0.1f, 0.03f, 0.09f, 1f),
                new Color(1f, 0.62f, 0.84f, 1f)
            ),
            enemy: Enemy(
                new Color(0.92f, 0.43f, 0.66f, 1f),
                new Color(0.58f, 0.15f, 0.45f, 1f),
                1.18f
            ),
            timeBetweenRings: 2.9f,
            enemyCount: 34,
            Pair(Spawner.RingType.Additive, 12, Spawner.RingType.Multiplier, 4),
            Pair(Spawner.RingType.Reducer, 7, Spawner.RingType.Additive, 15),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 14),
            Pair(Spawner.RingType.Additive, 18, Spawner.RingType.Reducer, 5),
            Pair(Spawner.RingType.Multiplier, 6, Spawner.RingType.Additive, 16),
            Pair(Spawner.RingType.Reducer, 8, Spawner.RingType.Multiplier, 4),
            Pair(Spawner.RingType.Additive, 20, Spawner.RingType.Additive, 12),
            Pair(Spawner.RingType.Multiplier, 7, Spawner.RingType.Reducer, 6)
        ),
        Level(
            theme: Theme(
                new Color(0.45f, 0.96f, 1f, 0f),
                new Color(0.6f, 0.99f, 1f, 0.45f),
                new Color(0.57f, 0.83f, 0.93f, 0.6f)
            ),
            env: Env(
                new Color(0.08f, 0.18f, 0.24f, 1f),
                new Color(0.1f, 0.2f, 0.27f, 1f),
                new Color(0.05f, 0.12f, 0.17f, 1f),
                new Color(0.85f, 0.98f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.6f, 0.99f, 1f, 1f),
                new Color(0.24f, 0.67f, 0.84f, 1f),
                1.02f
            ),
            timeBetweenRings: 2.95f,
            enemyCount: 36,
            Pair(Spawner.RingType.Multiplier, 4, Spawner.RingType.Additive, 16),
            Pair(Spawner.RingType.Additive, 18, Spawner.RingType.Reducer, 7),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 20),
            Pair(Spawner.RingType.Reducer, 9, Spawner.RingType.Additive, 14),
            Pair(Spawner.RingType.Additive, 22, Spawner.RingType.Multiplier, 6),
            Pair(Spawner.RingType.Reducer, 7, Spawner.RingType.Additive, 24),
            Pair(Spawner.RingType.Multiplier, 6, Spawner.RingType.Additive, 12),
            Pair(Spawner.RingType.Additive, 25, Spawner.RingType.Reducer, 8)
        ),
        Level(
            theme: Theme(
                new Color(0.62f, 0.32f, 0.95f, 0f),
                new Color(0.78f, 0.43f, 1f, 0.45f),
                new Color(0.48f, 0.26f, 0.74f, 0.6f)
            ),
            env: Env(
                new Color(0.08f, 0.05f, 0.19f, 1f),
                new Color(0.07f, 0.05f, 0.16f, 1f),
                new Color(0.03f, 0.02f, 0.09f, 1f),
                new Color(0.89f, 0.74f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.78f, 0.43f, 1f, 1f),
                new Color(0.42f, 0.18f, 0.76f, 1f),
                1.12f
            ),
            timeBetweenRings: 3f,
            enemyCount: 38,
            Pair(Spawner.RingType.Additive, 14, Spawner.RingType.Multiplier, 5),
            Pair(Spawner.RingType.Reducer, 8, Spawner.RingType.Additive, 18),
            Pair(Spawner.RingType.Multiplier, 6, Spawner.RingType.Additive, 16),
            Pair(Spawner.RingType.Additive, 24, Spawner.RingType.Reducer, 7),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Multiplier, 4),
            Pair(Spawner.RingType.Additive, 26, Spawner.RingType.Reducer, 9),
            Pair(Spawner.RingType.Multiplier, 7, Spawner.RingType.Additive, 20),
            Pair(Spawner.RingType.Reducer, 10, Spawner.RingType.Additive, 28),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 22)
        ),
        Level(
            theme: Theme(
                new Color(0.98f, 0.82f, 0.26f, 0f),
                new Color(0.99f, 0.92f, 0.36f, 0.45f),
                new Color(0.92f, 0.86f, 0.48f, 0.6f)
            ),
            env: Env(
                new Color(0.28f, 0.26f, 0.1f, 1f),
                new Color(0.25f, 0.22f, 0.09f, 1f),
                new Color(0.18f, 0.16f, 0.05f, 1f),
                new Color(1f, 0.96f, 0.62f, 1f)
            ),
            enemy: Enemy(
                new Color(1f, 0.88f, 0.33f, 1f),
                new Color(0.96f, 0.63f, 0.15f, 1f),
                1f
            ),
            timeBetweenRings: 3.05f,
            enemyCount: 40,
            Pair(Spawner.RingType.Additive, 15, Spawner.RingType.Multiplier, 5),
            Pair(Spawner.RingType.Reducer, 9, Spawner.RingType.Additive, 20),
            Pair(Spawner.RingType.Multiplier, 7, Spawner.RingType.Additive, 18),
            Pair(Spawner.RingType.Additive, 26, Spawner.RingType.Reducer, 8),
            Pair(Spawner.RingType.Multiplier, 6, Spawner.RingType.Additive, 24),
            Pair(Spawner.RingType.Reducer, 11, Spawner.RingType.Additive, 22),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 28),
            Pair(Spawner.RingType.Additive, 30, Spawner.RingType.Reducer, 10),
            Pair(Spawner.RingType.Multiplier, 8, Spawner.RingType.Additive, 26)
        ),
        Level(
            theme: Theme(
                new Color(0.18f, 0.52f, 0.95f, 0f),
                new Color(0.31f, 0.67f, 1f, 0.45f),
                new Color(0.24f, 0.49f, 0.78f, 0.6f)
            ),
            env: Env(
                new Color(0.09f, 0.15f, 0.28f, 1f),
                new Color(0.08f, 0.14f, 0.25f, 1f),
                new Color(0.04f, 0.08f, 0.17f, 1f),
                new Color(0.72f, 0.85f, 1f, 1f)
            ),
            enemy: Enemy(
                new Color(0.31f, 0.67f, 1f, 1f),
                new Color(0.12f, 0.38f, 0.74f, 1f),
                1.07f
            ),
            timeBetweenRings: 3.05f,
            enemyCount: 40,
            Pair(Spawner.RingType.Additive, 15, Spawner.RingType.Multiplier, 5),
            Pair(Spawner.RingType.Reducer, 9, Spawner.RingType.Additive, 20),
            Pair(Spawner.RingType.Multiplier, 7, Spawner.RingType.Additive, 18),
            Pair(Spawner.RingType.Additive, 26, Spawner.RingType.Reducer, 8),
            Pair(Spawner.RingType.Multiplier, 6, Spawner.RingType.Additive, 24),
            Pair(Spawner.RingType.Reducer, 11, Spawner.RingType.Additive, 22),
            Pair(Spawner.RingType.Multiplier, 5, Spawner.RingType.Additive, 28),
            Pair(Spawner.RingType.Additive, 30, Spawner.RingType.Reducer, 10),
            Pair(Spawner.RingType.Multiplier, 8, Spawner.RingType.Additive, 26)
        )
    };

    public static int LevelCount => Levels.Length;

    public static LevelDefinition GetLevel(int levelIndex)
    {
        if (Levels == null || Levels.Length == 0)
        {
            return null;
        }

        if (levelIndex < 0)
        {
            levelIndex = 0;
        }

        int index = levelIndex % Levels.Length;
        return Levels[index];
    }

    public static GameManager.Colors[] GetAllThemes()
    {
        if (Levels == null || Levels.Length == 0)
        {
            return System.Array.Empty<GameManager.Colors>();
        }

        var themes = new GameManager.Colors[Levels.Length];
        for (int i = 0; i < Levels.Length; i++)
        {
            var source = Levels[i].Theme;
            themes[i] = new GameManager.Colors
            {
                RingColor = source.RingColor,
                RingTransColor = source.RingTransColor,
                PlatformColor = source.PlatformColor
            };
        }

        return themes;
    }

    private static LevelDefinition Level(GameManager.Colors theme, EnvironmentSettings env, EnemyStyle enemy, float timeBetweenRings, int enemyCount, params RingPair[] rings)
    {
        return new LevelDefinition
        {
            Theme = theme,
            Environment = env,
            Enemy = enemy,
            TimeBetweenRings = timeBetweenRings,
            EnemyCount = enemyCount,
            Rings = rings
        };
    }

    private static GameManager.Colors Theme(Color ring, Color ringTrans, Color platform)
    {
        return new GameManager.Colors
        {
            RingColor = ring,
            RingTransColor = ringTrans,
            PlatformColor = platform
        };
    }

    private static EnvironmentSettings Env(Color fog, Color ambient, Color camera, Color light)
    {
        return new EnvironmentSettings
        {
            FogColor = fog,
            AmbientColor = ambient,
            CameraColor = camera,
            LightColor = light
        };
    }

    private static EnemyStyle Enemy(Color primary, Color secondary, float scaleMultiplier = 1f)
    {
        return new EnemyStyle
        {
            PrimaryColor = primary,
            SecondaryColor = secondary,
            ScaleMultiplier = scaleMultiplier
        };
    }

    private static RingPair Pair(Spawner.RingType firstType, int firstEffect, Spawner.RingType secondType, int secondEffect)
    {
        return new RingPair
        {
            Inner = new RingData { RingType = firstType, Effect = firstEffect },
            Outer = new RingData { RingType = secondType, Effect = secondEffect }
        };
    }
}

