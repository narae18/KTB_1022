namespace PuxxeStudio
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System;



    [System.Diagnostics.CodeAnalysis.SuppressMessage("WarningType", "CS0108")]
    public class PlayerControllDemo : MonoBehaviour
    {
        public float lookSensitivity;
        public float cameraRotationLimit;
        private float currentCameraRotationX;

        public GameObject TheCamera;
        public Camera walkCamera;
        public Camera runCamera;
        public Camera minimapCamera;
        public Camera popupCamera;
        public Vector3 offset;

        public float jumpForce = 3f;
        public float moveSpeed = 8f;

        Rigidbody rigidbody;
        public GameObject CheckGround;

        public static float health;

        //------ 달리기 구현
        //1초내에 w키를 2번 누르면 달리기.
        private float timeDoubleClick = 1f;
        private float ClickStart;

        public bool EnableRunning = true;

        //------현재 플레이어 상태           		
        public bool isWalking = false; //플레이어가 걷는중인지
        public bool isWalking_LR = false; //플레이어가 좌우로 걷는중인지


        public bool isRunning = false; // 플레이어가 뛰는 중인지


        public bool isGrounded = false; //플레이어가 땅 위에 있는지
        bool isFalling = false; //플레이어가 떨어지는 중인지
        bool isJumping = false; //플레이어가 점프중인지
        bool EnableDoubleJumping = true; //플레이어가 더블점프가 가능한지
        bool isDoubleJumping = false; //플레이어가 더블점프 중인지


        int[] walkAnimation = { 21, 22, 23, 24 };
        int[] runAnimation = { 31, 22, 23, 24 };
        public int[] currentMoveAnimation = new int[4];

        //------에니메이션

        public Animator animator;
        Dictionary<string, int> actions = new Dictionary<string, int>();
        [HideInInspector]
        public int actionID = 0;
        [SerializeField]


        //------아이템 효과
        //public GameObject[] grenades;
        //public int hasGrenades;

        //스피드업아이템
        private float originalMoveSpeed; // 원래 이동 속도 (15초 후 복원하기 위해 저장)
        private float originalJumpForce;
        private Vector3 originalScale;


        public AudioSource source;
        public Vector3 minScale;
        public Vector3 maxScale;
        public AudioLoudnessDetection detector;

        public float loudnessSensibility = 100;
        public float threshold = 0.1f;


        bool animationHasLoop = false;
        bool actionNoLoopedReturnToIdle = true;
        AnimatorClipInfo[] animatorInfo;
        AnimationClip currentAnimationClip;
        string currentAnimation;
        [Header("Actions Names")]
        string A_001_POSE_1 = "001_pose_1";
        string A_002_POSE_2 = "002_pose_2";
        string A_003_POSE_3 = "003_pose_3";
        string A_004_POSE_4 = "004_pose_4";
        string A_005_POSE_5 = "005_pose_5";
        string A_006_POSE_6 = "006_pose_6";
        string A_007_POSE_7 = "007_pose_7";
        string A_008_POSE_8 = "008_pose_8";
        string A_009_POSE_9 = "009_pose_9";
        string A_010_POSE_10 = "010_pose_10";
        string A_011_IDLE_1 = "011_idle_1";
        string A_012_IDLE_2 = "012_idle_2";
        string A_013_IDLE_3 = "013_idle_3";
        string A_014_IDLE_4 = "014_idle_4";
        string A_015_IDLE_5 = "015_idle_5";
        string A_016_IDLE_6 = "016_idle_6";
        string A_017_IDLE_7 = "017_idle_7";
        string A_018_IDLE_8 = "018_idle_8";
        string A_019_IDLE_9 = "019_idle_9";
        string A_020_IDLE_10 = "020_idle_10";
        string A_021_WALK_1 = "021_walk_1";
        string A_022_WALK_2 = "022_walk_2";
        string A_023_WALK_3 = "023_walk_3";
        string A_024_WALK_4 = "024_walk_4";
        string A_025_WALK_5 = "025_walk_5";
        string A_026_WALK_6 = "026_walk_6";
        string A_027_WALK_7 = "027_walk_7";
        string A_028_WALK_8 = "028_walk_8";
        string A_029_WALK_9 = "029_walk_9";
        string A_030_WALK_10 = "030_walk_10";
        string A_031_RUN_1 = "031_run_1";
        string A_032_RUN_2 = "032_run_2";
        string A_033_RUN_3 = "033_run_3";
        string A_034_RUN_4 = "034_run_4";
        string A_035_RUN_5 = "035_run_5";
        string A_036_RUN_6 = "036_run_6";
        string A_037_RUN_7 = "037_run_7";
        string A_038_RUN_8 = "038_run_8";
        string A_039_RUN_9 = "039_run_9";
        string A_040_RUN_10 = "040_run_10";
        string A_041_JUMP_1 = "041_jump_1";
        string A_042_JUMP_2 = "042_jump_2";
        string A_043_JUMP_3 = "043_jump_3";
        string A_044_JUMP_4 = "044_jump_4";
        string A_045_JUMP_5 = "045_jump_5";
        string A_046_JUMP_6 = "046_jump_6";
        string A_047_JUMP_7 = "047_jump_7";
        string A_048_JUMP_8 = "048_jump_8";
        string A_049_JUMP_9 = "049_jump_9";
        string A_050_JUMP_10 = "050_jump_10";
        string A_051_JUMP_SPIN_1 = "051_jump_spin_1";
        string A_052_JUMP_SPIN_2 = "052_jump_spin_2";
        string A_053_JUMP_SPIN_3 = "053_jump_spin_3";
        string A_054_JUMP_SPIN_4 = "054_jump_spin_4";
        string A_055_JUMP_SPIN_5 = "055_jump_spin_5";
        string A_056_JUMP_SPIN_6 = "056_jump_spin_6";
        string A_057_JUMP_SPIN_7 = "057_jump_spin_7";
        string A_058_JUMP_SPIN_8 = "058_jump_spin_8";
        string A_059_JUMP_SPIN_9 = "059_jump_spin_9";
        string A_060_JUMP_SPIN_10 = "060_jump_spin_10";
        string A_061_FALL_1 = "061_fall_1";
        string A_062_FALL_2 = "062_fall_2";
        string A_063_FALL_3 = "063_fall_3";
        string A_064_FALL_4 = "064_fall_4";
        string A_065_FALL_5 = "065_fall_5";
        string A_066_FALL_6 = "066_fall_6";
        string A_067_FALL_7 = "067_fall_7";
        string A_068_FALL_8 = "068_fall_8";
        string A_069_FALL_9 = "069_fall_9";
        string A_070_FALL_10 = "070_fall_10";
        string A_071_LAND_1 = "071_land_1";
        string A_072_LAND_2 = "072_land_2";
        string A_073_LAND_3 = "073_land_3";
        string A_074_LAND_4 = "074_land_4";
        string A_075_LAND_5 = "075_land_5";
        string A_076_LAND_6 = "076_land_6";
        string A_077_LAND_7 = "077_land_7";
        string A_078_LAND_8 = "078_land_8";
        string A_079_LAND_9 = "079_land_9";
        string A_080_LAND_10 = "080_land_10";
        string A_081_PUNCH_1 = "081_punch_1";
        string A_082_PUNCH_2 = "082_punch_2";
        string A_083_PUNCH_3 = "083_punch_3";
        string A_084_PUNCH_4 = "084_punch_4";
        string A_085_PUNCH_5 = "085_punch_5";
        string A_086_PUNCH_6 = "086_punch_6";
        string A_087_PUNCH_7 = "087_punch_7";
        string A_088_PUNCH_8 = "088_punch_8";
        string A_089_PUNCH_9 = "089_punch_9";
        string A_090_PUNCH_10 = "090_punch_10";
        string A_091_KICK_1 = "091_kick_1";
        string A_092_KICK_2 = "092_kick_2";
        string A_093_KICK_3 = "093_kick_3";
        string A_094_KICK_4 = "094_kick_4";
        string A_095_KICK_5 = "095_kick_5";
        string A_096_KICK_6 = "096_kick_6";
        string A_097_KICK_7 = "097_kick_7";
        string A_098_KICK_8 = "098_kick_8";
        string A_099_KICK_9 = "099_kick_9";
        string A_100_KICK_10 = "100_kick_10";
        string A_101_ATTACK_1 = "101_attack_1";
        string A_102_ATTACK_2 = "102_attack_2";
        string A_103_ATTACK_3 = "103_attack_3";
        string A_104_ATTACK_4 = "104_attack_4";
        string A_105_ATTACK_5 = "105_attack_5";
        string A_106_ATTACK_6 = "106_attack_6";
        string A_107_ATTACK_7 = "107_attack_7";
        string A_108_ATTACK_8 = "108_attack_8";
        string A_109_ATTACK_9 = "109_attack_9";
        string A_110_ATTACK_10 = "110_attack_10";
        string A_111_MAGIC_1 = "111_magic_1";
        string A_112_MAGIC_2 = "112_magic_2";
        string A_113_MAGIC_3 = "113_magic_3";
        string A_114_MAGIC_4 = "114_magic_4";
        string A_115_MAGIC_5 = "115_magic_5";
        string A_116_MAGIC_6 = "116_magic_6";
        string A_117_MAGIC_7 = "117_magic_7";
        string A_118_MAGIC_8 = "118_magic_8";
        string A_119_MAGIC_9 = "119_magic_9";
        string A_120_MAGIC_10 = "120_magic_10";
        string A_121_BLOCK_1 = "121_block_1";
        string A_122_BLOCK_2 = "122_block_2";
        string A_123_BLOCK_3 = "123_block_3";
        string A_124_BLOCK_4 = "124_block_4";
        string A_125_BLOCK_5 = "125_block_5";
        string A_126_BLOCK_6 = "126_block_6";
        string A_127_BLOCK_7 = "127_block_7";
        string A_128_BLOCK_8 = "128_block_8";
        string A_129_BLOCK_9 = "129_block_9";
        string A_130_BLOCK_10 = "130_block_10";
        string A_131_HIT_1 = "131_hit_1";
        string A_132_HIT_2 = "132_hit_2";
        string A_133_HIT_3 = "133_hit_3";
        string A_134_HIT_4 = "134_hit_4";
        string A_135_HIT_5 = "135_hit_5";
        string A_136_HIT_6 = "136_hit_6";
        string A_137_HIT_7 = "137_hit_7";
        string A_138_HIT_8 = "138_hit_8";
        string A_139_HIT_9 = "139_hit_9";
        string A_140_HIT_10 = "140_hit_10";
        string A_141_LOSE_1 = "141_lose_1";
        string A_142_LOSE_2 = "142_lose_2";
        string A_143_LOSE_3 = "143_lose_3";
        string A_144_LOSE_4 = "144_lose_4";
        string A_145_LOSE_5 = "145_lose_5";
        string A_146_LOSE_6 = "146_lose_6";
        string A_147_LOSE_7 = "147_lose_7";
        string A_148_LOSE_8 = "148_lose_8";
        string A_149_LOSE_9 = "149_lose_9";
        string A_150_LOSE_10 = "150_lose_10";
        string A_151_DIE_1 = "151_die_1";
        string A_152_DIE_2 = "152_die_2";
        string A_153_DIE_3 = "153_die_3";
        string A_154_DIE_4 = "154_die_4";
        string A_155_DIE_5 = "155_die_5";
        string A_156_DIE_6 = "156_die_6";
        string A_157_DIE_7 = "157_die_7";
        string A_158_DIE_8 = "158_die_8";
        string A_159_DIE_9 = "159_die_9";
        string A_160_DIE_10 = "160_die_10";
        string A_161_VICTORY_1 = "161_victory_1";
        string A_162_VICTORY_2 = "162_victory_2";
        string A_163_VICTORY_3 = "163_victory_3";
        string A_164_VICTORY_4 = "164_victory_4";
        string A_165_VICTORY_5 = "165_victory_5";
        string A_166_VICTORY_6 = "166_victory_6";
        string A_167_VICTORY_7 = "167_victory_7";
        string A_168_VICTORY_8 = "168_victory_8";
        string A_169_VICTORY_9 = "169_victory_9";
        string A_170_VICTORY_10 = "170_victory_10";
        string A_171_EXTRA_ACTIONS_1 = "171_extra_actions_1";
        string A_172_EXTRA_ACTIONS_2 = "172_extra_actions_2";
        string A_173_EXTRA_ACTIONS_3 = "173_extra_actions_3";
        string A_174_EXTRA_ACTIONS_4 = "174_extra_actions_4";
        string A_175_EXTRA_ACTIONS_5 = "175_extra_actions_5";
        string A_176_EXTRA_ACTIONS_6 = "176_extra_actions_6";
        string A_177_EXTRA_ACTIONS_7 = "177_extra_actions_7";
        string A_178_EXTRA_ACTIONS_8 = "178_extra_actions_8";
        string A_179_EXTRA_ACTIONS_9 = "179_extra_actions_9";
        string A_180_EXTRA_ACTIONS_10 = "180_extra_actions_10";
        string A_181_CUSTOM_POSE_1 = "181_custom_pose_1";
        string A_182_CUSTOM_POSE_2 = "182_custom_pose_2";
        string A_183_CUSTOM_POSE_3 = "183_custom_pose_3";
        string A_184_CUSTOM_POSE_4 = "184_custom_pose_4";
        string A_185_CUSTOM_POSE_5 = "185_custom_pose_5";
        string A_186_CUSTOM_POSE_6 = "186_custom_pose_6";
        string A_187_CUSTOM_POSE_7 = "187_custom_pose_7";
        string A_188_CUSTOM_POSE_8 = "188_custom_pose_8";
        string A_189_CUSTOM_POSE_9 = "189_custom_pose_9";
        string A_190_CUSTOM_POSE_10 = "190_custom_pose_10";
        string A_191_CUSTOM_IDLE_1 = "191_custom_idle_1";
        string A_192_CUSTOM_IDLE_2 = "192_custom_idle_2";
        string A_193_CUSTOM_IDLE_3 = "193_custom_idle_3";
        string A_194_CUSTOM_IDLE_4 = "194_custom_idle_4";
        string A_195_CUSTOM_IDLE_5 = "195_custom_idle_5";
        string A_196_CUSTOM_IDLE_6 = "196_custom_idle_6";
        string A_197_CUSTOM_IDLE_7 = "197_custom_idle_7";
        string A_198_CUSTOM_IDLE_8 = "198_custom_idle_8";
        string A_199_CUSTOM_IDLE_9 = "199_custom_idle_9";
        string A_200_CUSTOM_IDLE_10 = "200_custom_idle_10";
        string A_201_CUSTOM_WALK_1 = "201_custom_walk_1";
        string A_202_CUSTOM_WALK_2 = "202_custom_walk_2";
        string A_203_CUSTOM_WALK_3 = "203_custom_walk_3";
        string A_204_CUSTOM_WALK_4 = "204_custom_walk_4";
        string A_205_CUSTOM_WALK_5 = "205_custom_walk_5";
        string A_206_CUSTOM_WALK_6 = "206_custom_walk_6";
        string A_207_CUSTOM_WALK_7 = "207_custom_walk_7";
        string A_208_CUSTOM_WALK_8 = "208_custom_walk_8";
        string A_209_CUSTOM_WALK_9 = "209_custom_walk_9";
        string A_210_CUSTOM_WALK_10 = "210_custom_walk_10";
        string A_211_CUSTOM_RUN_1 = "211_custom_run_1";
        string A_212_CUSTOM_RUN_2 = "212_custom_run_2";
        string A_213_CUSTOM_RUN_3 = "213_custom_run_3";
        string A_214_CUSTOM_RUN_4 = "214_custom_run_4";
        string A_215_CUSTOM_RUN_5 = "215_custom_run_5";
        string A_216_CUSTOM_RUN_6 = "216_custom_run_6";
        string A_217_CUSTOM_RUN_7 = "217_custom_run_7";
        string A_218_CUSTOM_RUN_8 = "218_custom_run_8";
        string A_219_CUSTOM_RUN_9 = "219_custom_run_9";
        string A_220_CUSTOM_RUN_10 = "220_custom_run_10";
        string A_221_CUSTOM_JUMP_1 = "221_custom_jump_1";
        string A_222_CUSTOM_JUMP_2 = "222_custom_jump_2";
        string A_223_CUSTOM_JUMP_3 = "223_custom_jump_3";
        string A_224_CUSTOM_JUMP_4 = "224_custom_jump_4";
        string A_225_CUSTOM_JUMP_5 = "225_custom_jump_5";
        string A_226_CUSTOM_JUMP_6 = "226_custom_jump_6";
        string A_227_CUSTOM_JUMP_7 = "227_custom_jump_7";
        string A_228_CUSTOM_JUMP_8 = "228_custom_jump_8";
        string A_229_CUSTOM_JUMP_9 = "229_custom_jump_9";
        string A_230_CUSTOM_JUMP_10 = "230_custom_jump_10";
        string A_231_CUSTOM_JUMP_SPIN_1 = "231_custom_jump_spin_1";
        string A_232_CUSTOM_JUMP_SPIN_2 = "232_custom_jump_spin_2";
        string A_233_CUSTOM_JUMP_SPIN_3 = "233_custom_jump_spin_3";
        string A_234_CUSTOM_JUMP_SPIN_4 = "234_custom_jump_spin_4";
        string A_235_CUSTOM_JUMP_SPIN_5 = "235_custom_jump_spin_5";
        string A_236_CUSTOM_JUMP_SPIN_6 = "236_custom_jump_spin_6";
        string A_237_CUSTOM_JUMP_SPIN_7 = "237_custom_jump_spin_7";
        string A_238_CUSTOM_JUMP_SPIN_8 = "238_custom_jump_spin_8";
        string A_239_CUSTOM_JUMP_SPIN_9 = "239_custom_jump_spin_9";
        string A_240_CUSTOM_JUMP_SPIN_10 = "240_custom_jump_spin_10";
        string A_241_CUSTOM_FALL_1 = "241_custom_fall_1";
        string A_242_CUSTOM_FALL_2 = "242_custom_fall_2";
        string A_243_CUSTOM_FALL_3 = "243_custom_fall_3";
        string A_244_CUSTOM_FALL_4 = "244_custom_fall_4";
        string A_245_CUSTOM_FALL_5 = "245_custom_fall_5";
        string A_246_CUSTOM_FALL_6 = "246_custom_fall_6";
        string A_247_CUSTOM_FALL_7 = "247_custom_fall_7";
        string A_248_CUSTOM_FALL_8 = "248_custom_fall_8";
        string A_249_CUSTOM_FALL_9 = "249_custom_fall_9";
        string A_250_CUSTOM_FALL_10 = "250_custom_fall_10";
        string A_251_CUSTOM_LAND_1 = "251_custom_land_1";
        string A_252_CUSTOM_LAND_2 = "252_custom_land_2";
        string A_253_CUSTOM_LAND_3 = "253_custom_land_3";
        string A_254_CUSTOM_LAND_4 = "254_custom_land_4";
        string A_255_CUSTOM_LAND_5 = "255_custom_land_5";
        string A_256_CUSTOM_LAND_6 = "256_custom_land_6";
        string A_257_CUSTOM_LAND_7 = "257_custom_land_7";
        string A_258_CUSTOM_LAND_8 = "258_custom_land_8";
        string A_259_CUSTOM_LAND_9 = "259_custom_land_9";
        string A_260_CUSTOM_LAND_10 = "260_custom_land_10";
        string A_261_CUSTOM_PUNCH_1 = "261_custom_punch_1";
        string A_262_CUSTOM_PUNCH_2 = "262_custom_punch_2";
        string A_263_CUSTOM_PUNCH_3 = "263_custom_punch_3";
        string A_264_CUSTOM_PUNCH_4 = "264_custom_punch_4";
        string A_265_CUSTOM_PUNCH_5 = "265_custom_punch_5";
        string A_266_CUSTOM_PUNCH_6 = "266_custom_punch_6";
        string A_267_CUSTOM_PUNCH_7 = "267_custom_punch_7";
        string A_268_CUSTOM_PUNCH_8 = "268_custom_punch_8";
        string A_269_CUSTOM_PUNCH_9 = "269_custom_punch_9";
        string A_270_CUSTOM_PUNCH_10 = "270_custom_punch_10";
        string A_271_CUSTOM_KICK_1 = "271_custom_kick_1";
        string A_272_CUSTOM_KICK_2 = "272_custom_kick_2";
        string A_273_CUSTOM_KICK_3 = "273_custom_kick_3";
        string A_274_CUSTOM_KICK_4 = "274_custom_kick_4";
        string A_275_CUSTOM_KICK_5 = "275_custom_kick_5";
        string A_276_CUSTOM_KICK_6 = "276_custom_kick_6";
        string A_277_CUSTOM_KICK_7 = "277_custom_kick_7";
        string A_278_CUSTOM_KICK_8 = "278_custom_kick_8";
        string A_279_CUSTOM_KICK_9 = "279_custom_kick_9";
        string A_280_CUSTOM_KICK_10 = "280_custom_kick_10";
        string A_281_CUSTOM_ATTACK_1 = "281_custom_attack_1";
        string A_282_CUSTOM_ATTACK_2 = "282_custom_attack_2";
        string A_283_CUSTOM_ATTACK_3 = "283_custom_attack_3";
        string A_284_CUSTOM_ATTACK_4 = "284_custom_attack_4";
        string A_285_CUSTOM_ATTACK_5 = "285_custom_attack_5";
        string A_286_CUSTOM_ATTACK_6 = "286_custom_attack_6";
        string A_287_CUSTOM_ATTACK_7 = "287_custom_attack_7";
        string A_288_CUSTOM_ATTACK_8 = "288_custom_attack_8";
        string A_289_CUSTOM_ATTACK_9 = "289_custom_attack_9";
        string A_290_CUSTOM_ATTACK_10 = "290_custom_attack_10";
        string A_291_CUSTOM_MAGIC_1 = "291_custom_magic_1";
        string A_292_CUSTOM_MAGIC_2 = "292_custom_magic_2";
        string A_293_CUSTOM_MAGIC_3 = "293_custom_magic_3";
        string A_294_CUSTOM_MAGIC_4 = "294_custom_magic_4";
        string A_295_CUSTOM_MAGIC_5 = "295_custom_magic_5";
        string A_296_CUSTOM_MAGIC_6 = "296_custom_magic_6";
        string A_297_CUSTOM_MAGIC_7 = "297_custom_magic_7";
        string A_298_CUSTOM_MAGIC_8 = "298_custom_magic_8";
        string A_299_CUSTOM_MAGIC_9 = "299_custom_magic_9";
        string A_300_CUSTOM_MAGIC_10 = "300_custom_magic_10";
        string A_301_CUSTOM_BLOCK_1 = "301_custom_block_1";
        string A_302_CUSTOM_BLOCK_2 = "302_custom_block_2";
        string A_303_CUSTOM_BLOCK_3 = "303_custom_block_3";
        string A_304_CUSTOM_BLOCK_4 = "304_custom_block_4";
        string A_305_CUSTOM_BLOCK_5 = "305_custom_block_5";
        string A_306_CUSTOM_BLOCK_6 = "306_custom_block_6";
        string A_307_CUSTOM_BLOCK_7 = "307_custom_block_7";
        string A_308_CUSTOM_BLOCK_8 = "308_custom_block_8";
        string A_309_CUSTOM_BLOCK_9 = "309_custom_block_9";
        string A_310_CUSTOM_BLOCK_10 = "310_custom_block_10";
        string A_311_CUSTOM_HIT_1 = "311_custom_hit_1";
        string A_312_CUSTOM_HIT_2 = "312_custom_hit_2";
        string A_313_CUSTOM_HIT_3 = "313_custom_hit_3";
        string A_314_CUSTOM_HIT_4 = "314_custom_hit_4";
        string A_315_CUSTOM_HIT_5 = "315_custom_hit_5";
        string A_316_CUSTOM_HIT_6 = "316_custom_hit_6";
        string A_317_CUSTOM_HIT_7 = "317_custom_hit_7";
        string A_318_CUSTOM_HIT_8 = "318_custom_hit_8";
        string A_319_CUSTOM_HIT_9 = "319_custom_hit_9";
        string A_320_CUSTOM_HIT_10 = "320_custom_hit_10";
        string A_321_CUSTOM_LOSE_1 = "321_custom_lose_1";
        string A_322_CUSTOM_LOSE_2 = "322_custom_lose_2";
        string A_323_CUSTOM_LOSE_3 = "323_custom_lose_3";
        string A_324_CUSTOM_LOSE_4 = "324_custom_lose_4";
        string A_325_CUSTOM_LOSE_5 = "325_custom_lose_5";
        string A_326_CUSTOM_LOSE_6 = "326_custom_lose_6";
        string A_327_CUSTOM_LOSE_7 = "327_custom_lose_7";
        string A_328_CUSTOM_LOSE_8 = "328_custom_lose_8";
        string A_329_CUSTOM_LOSE_9 = "329_custom_lose_9";
        string A_330_CUSTOM_LOSE_10 = "330_custom_lose_10";
        string A_331_CUSTOM_DIE_1 = "331_custom_die_1";
        string A_332_CUSTOM_DIE_2 = "332_custom_die_2";
        string A_333_CUSTOM_DIE_3 = "333_custom_die_3";
        string A_334_CUSTOM_DIE_4 = "334_custom_die_4";
        string A_335_CUSTOM_DIE_5 = "335_custom_die_5";
        string A_336_CUSTOM_DIE_6 = "336_custom_die_6";
        string A_337_CUSTOM_DIE_7 = "337_custom_die_7";
        string A_338_CUSTOM_DIE_8 = "338_custom_die_8";
        string A_339_CUSTOM_DIE_9 = "339_custom_die_9";
        string A_340_CUSTOM_DIE_10 = "340_custom_die_10";
        string A_341_CUSTOM_VICTORY_1 = "341_custom_victory_1";
        string A_342_CUSTOM_VICTORY_2 = "342_custom_victory_2";
        string A_343_CUSTOM_VICTORY_3 = "343_custom_victory_3";
        string A_344_CUSTOM_VICTORY_4 = "344_custom_victory_4";
        string A_345_CUSTOM_VICTORY_5 = "345_custom_victory_5";
        string A_346_CUSTOM_VICTORY_6 = "346_custom_victory_6";
        string A_347_CUSTOM_VICTORY_7 = "347_custom_victory_7";
        string A_348_CUSTOM_VICTORY_8 = "348_custom_victory_8";
        string A_349_CUSTOM_VICTORY_9 = "349_custom_victory_9";
        string A_350_CUSTOM_VICTORY_10 = "350_custom_victory_10";
        string A_351_CUSTOM_EXTRA_ACTIONS_1 = "351_custom_extra_actions_1";
        string A_352_CUSTOM_EXTRA_ACTIONS_2 = "352_custom_extra_actions_2";
        string A_353_CUSTOM_EXTRA_ACTIONS_3 = "353_custom_extra_actions_3";
        string A_354_CUSTOM_EXTRA_ACTIONS_4 = "354_custom_extra_actions_4";
        string A_355_CUSTOM_EXTRA_ACTIONS_5 = "355_custom_extra_actions_5";
        string A_356_CUSTOM_EXTRA_ACTIONS_6 = "356_custom_extra_actions_6";
        string A_357_CUSTOM_EXTRA_ACTIONS_7 = "357_custom_extra_actions_7";
        string A_358_CUSTOM_EXTRA_ACTIONS_8 = "358_custom_extra_actions_8";
        string A_359_CUSTOM_EXTRA_ACTIONS_9 = "359_custom_extra_actions_9";
        string A_360_CUSTOM_EXTRA_ACTIONS_10 = "360_custom_extra_actions_10";
        [Header("Actions ID")]
        int A_001_POSE_1_ID = 1;
        int A_002_POSE_2_ID = 2;
        int A_003_POSE_3_ID = 3;
        int A_004_POSE_4_ID = 4;
        int A_005_POSE_5_ID = 5;
        int A_006_POSE_6_ID = 6;
        int A_007_POSE_7_ID = 7;
        int A_008_POSE_8_ID = 8;
        int A_009_POSE_9_ID = 9;
        int A_010_POSE_10_ID = 10;
        int A_011_IDLE_1_ID = 11;
        int A_012_IDLE_2_ID = 12;
        int A_013_IDLE_3_ID = 13;
        int A_014_IDLE_4_ID = 14;
        int A_015_IDLE_5_ID = 15;
        int A_016_IDLE_6_ID = 16;
        int A_017_IDLE_7_ID = 17;
        int A_018_IDLE_8_ID = 18;
        int A_019_IDLE_9_ID = 19;
        int A_020_IDLE_10_ID = 20;
        int A_021_WALK_1_ID = 21;
        int A_022_WALK_2_ID = 22;
        int A_023_WALK_3_ID = 23;
        int A_024_WALK_4_ID = 24;
        int A_025_WALK_5_ID = 25;
        int A_026_WALK_6_ID = 26;
        int A_027_WALK_7_ID = 27;
        int A_028_WALK_8_ID = 28;
        int A_029_WALK_9_ID = 29;
        int A_030_WALK_10_ID = 30;
        int A_031_RUN_1_ID = 31;
        int A_032_RUN_2_ID = 32;
        int A_033_RUN_3_ID = 33;
        int A_034_RUN_4_ID = 34;
        int A_035_RUN_5_ID = 35;
        int A_036_RUN_6_ID = 36;
        int A_037_RUN_7_ID = 37;
        int A_038_RUN_8_ID = 38;
        int A_039_RUN_9_ID = 39;
        int A_040_RUN_10_ID = 40;
        int A_041_JUMP_1_ID = 41;
        int A_042_JUMP_2_ID = 42;
        int A_043_JUMP_3_ID = 43;
        int A_044_JUMP_4_ID = 44;
        int A_045_JUMP_5_ID = 45;
        int A_046_JUMP_6_ID = 46;
        int A_047_JUMP_7_ID = 47;
        int A_048_JUMP_8_ID = 48;
        int A_049_JUMP_9_ID = 49;
        int A_050_JUMP_10_ID = 50;
        int A_051_JUMP_SPIN_1_ID = 51;
        int A_052_JUMP_SPIN_2_ID = 52;
        int A_053_JUMP_SPIN_3_ID = 53;
        int A_054_JUMP_SPIN_4_ID = 54;
        int A_055_JUMP_SPIN_5_ID = 55;
        int A_056_JUMP_SPIN_6_ID = 56;
        int A_057_JUMP_SPIN_7_ID = 57;
        int A_058_JUMP_SPIN_8_ID = 58;
        int A_059_JUMP_SPIN_9_ID = 59;
        int A_060_JUMP_SPIN_10_ID = 60;
        int A_061_FALL_1_ID = 61;
        int A_062_FALL_2_ID = 62;
        int A_063_FALL_3_ID = 63;
        int A_064_FALL_4_ID = 64;
        int A_065_FALL_5_ID = 65;
        int A_066_FALL_6_ID = 66;
        int A_067_FALL_7_ID = 67;
        int A_068_FALL_8_ID = 68;
        int A_069_FALL_9_ID = 69;
        int A_070_FALL_10_ID = 70;
        int A_071_LAND_1_ID = 71;
        int A_072_LAND_2_ID = 72;
        int A_073_LAND_3_ID = 73;
        int A_074_LAND_4_ID = 74;
        int A_075_LAND_5_ID = 75;
        int A_076_LAND_6_ID = 76;
        int A_077_LAND_7_ID = 77;
        int A_078_LAND_8_ID = 78;
        int A_079_LAND_9_ID = 79;
        int A_080_LAND_10_ID = 80;
        int A_081_PUNCH_1_ID = 81;
        int A_082_PUNCH_2_ID = 82;
        int A_083_PUNCH_3_ID = 83;
        int A_084_PUNCH_4_ID = 84;
        int A_085_PUNCH_5_ID = 85;
        int A_086_PUNCH_6_ID = 86;
        int A_087_PUNCH_7_ID = 87;
        int A_088_PUNCH_8_ID = 88;
        int A_089_PUNCH_9_ID = 89;
        int A_090_PUNCH_10_ID = 90;
        int A_091_KICK_1_ID = 91;
        int A_092_KICK_2_ID = 92;
        int A_093_KICK_3_ID = 93;
        int A_094_KICK_4_ID = 94;
        int A_095_KICK_5_ID = 95;
        int A_096_KICK_6_ID = 96;
        int A_097_KICK_7_ID = 97;
        int A_098_KICK_8_ID = 98;
        int A_099_KICK_9_ID = 99;
        int A_100_KICK_10_ID = 100;
        int A_101_ATTACK_1_ID = 101;
        int A_102_ATTACK_2_ID = 102;
        int A_103_ATTACK_3_ID = 103;
        int A_104_ATTACK_4_ID = 104;
        int A_105_ATTACK_5_ID = 105;
        int A_106_ATTACK_6_ID = 106;
        int A_107_ATTACK_7_ID = 107;
        int A_108_ATTACK_8_ID = 108;
        int A_109_ATTACK_9_ID = 109;
        int A_110_ATTACK_10_ID = 110;
        int A_111_MAGIC_1_ID = 111;
        int A_112_MAGIC_2_ID = 112;
        int A_113_MAGIC_3_ID = 113;
        int A_114_MAGIC_4_ID = 114;
        int A_115_MAGIC_5_ID = 115;
        int A_116_MAGIC_6_ID = 116;
        int A_117_MAGIC_7_ID = 117;
        int A_118_MAGIC_8_ID = 118;
        int A_119_MAGIC_9_ID = 119;
        int A_120_MAGIC_10_ID = 120;
        int A_121_BLOCK_1_ID = 121;
        int A_122_BLOCK_2_ID = 122;
        int A_123_BLOCK_3_ID = 123;
        int A_124_BLOCK_4_ID = 124;
        int A_125_BLOCK_5_ID = 125;
        int A_126_BLOCK_6_ID = 126;
        int A_127_BLOCK_7_ID = 127;
        int A_128_BLOCK_8_ID = 128;
        int A_129_BLOCK_9_ID = 129;
        int A_130_BLOCK_10_ID = 130;
        int A_131_HIT_1_ID = 131;
        int A_132_HIT_2_ID = 132;
        int A_133_HIT_3_ID = 133;
        int A_134_HIT_4_ID = 134;
        int A_135_HIT_5_ID = 135;
        int A_136_HIT_6_ID = 136;
        int A_137_HIT_7_ID = 137;
        int A_138_HIT_8_ID = 138;
        int A_139_HIT_9_ID = 139;
        int A_140_HIT_10_ID = 140;
        int A_141_LOSE_1_ID = 141;
        int A_142_LOSE_2_ID = 142;
        int A_143_LOSE_3_ID = 143;
        int A_144_LOSE_4_ID = 144;
        int A_145_LOSE_5_ID = 145;
        int A_146_LOSE_6_ID = 146;
        int A_147_LOSE_7_ID = 147;
        int A_148_LOSE_8_ID = 148;
        int A_149_LOSE_9_ID = 149;
        int A_150_LOSE_10_ID = 150;
        int A_151_DIE_1_ID = 151;
        int A_152_DIE_2_ID = 152;
        int A_153_DIE_3_ID = 153;
        int A_154_DIE_4_ID = 154;
        int A_155_DIE_5_ID = 155;
        int A_156_DIE_6_ID = 156;
        int A_157_DIE_7_ID = 157;
        int A_158_DIE_8_ID = 158;
        int A_159_DIE_9_ID = 159;
        int A_160_DIE_10_ID = 160;
        int A_161_VICTORY_1_ID = 161;
        int A_162_VICTORY_2_ID = 162;
        int A_163_VICTORY_3_ID = 163;
        int A_164_VICTORY_4_ID = 164;
        int A_165_VICTORY_5_ID = 165;
        int A_166_VICTORY_6_ID = 166;
        int A_167_VICTORY_7_ID = 167;
        int A_168_VICTORY_8_ID = 168;
        int A_169_VICTORY_9_ID = 169;
        int A_170_VICTORY_10_ID = 170;
        int A_171_EXTRA_ACTIONS_1_ID = 171;
        int A_172_EXTRA_ACTIONS_2_ID = 172;
        int A_173_EXTRA_ACTIONS_3_ID = 173;
        int A_174_EXTRA_ACTIONS_4_ID = 174;
        int A_175_EXTRA_ACTIONS_5_ID = 175;
        int A_176_EXTRA_ACTIONS_6_ID = 176;
        int A_177_EXTRA_ACTIONS_7_ID = 177;
        int A_178_EXTRA_ACTIONS_8_ID = 178;
        int A_179_EXTRA_ACTIONS_9_ID = 179;
        int A_180_EXTRA_ACTIONS_10_ID = 180;
        int A_181_CUSTOM_POSE_1_ID = 181;
        int A_182_CUSTOM_POSE_2_ID = 182;
        int A_183_CUSTOM_POSE_3_ID = 183;
        int A_184_CUSTOM_POSE_4_ID = 184;
        int A_185_CUSTOM_POSE_5_ID = 185;
        int A_186_CUSTOM_POSE_6_ID = 186;
        int A_187_CUSTOM_POSE_7_ID = 187;
        int A_188_CUSTOM_POSE_8_ID = 188;
        int A_189_CUSTOM_POSE_9_ID = 189;
        int A_190_CUSTOM_POSE_10_ID = 190;
        int A_191_CUSTOM_IDLE_1_ID = 191;
        int A_192_CUSTOM_IDLE_2_ID = 192;
        int A_193_CUSTOM_IDLE_3_ID = 193;
        int A_194_CUSTOM_IDLE_4_ID = 194;
        int A_195_CUSTOM_IDLE_5_ID = 195;
        int A_196_CUSTOM_IDLE_6_ID = 196;
        int A_197_CUSTOM_IDLE_7_ID = 197;
        int A_198_CUSTOM_IDLE_8_ID = 198;
        int A_199_CUSTOM_IDLE_9_ID = 199;
        int A_200_CUSTOM_IDLE_10_ID = 200;
        int A_201_CUSTOM_WALK_1_ID = 201;
        int A_202_CUSTOM_WALK_2_ID = 202;
        int A_203_CUSTOM_WALK_3_ID = 203;
        int A_204_CUSTOM_WALK_4_ID = 204;
        int A_205_CUSTOM_WALK_5_ID = 205;
        int A_206_CUSTOM_WALK_6_ID = 206;
        int A_207_CUSTOM_WALK_7_ID = 207;
        int A_208_CUSTOM_WALK_8_ID = 208;
        int A_209_CUSTOM_WALK_9_ID = 209;
        int A_210_CUSTOM_WALK_10_ID = 210;
        int A_211_CUSTOM_RUN_1_ID = 211;
        int A_212_CUSTOM_RUN_2_ID = 212;
        int A_213_CUSTOM_RUN_3_ID = 213;
        int A_214_CUSTOM_RUN_4_ID = 214;
        int A_215_CUSTOM_RUN_5_ID = 215;
        int A_216_CUSTOM_RUN_6_ID = 216;
        int A_217_CUSTOM_RUN_7_ID = 217;
        int A_218_CUSTOM_RUN_8_ID = 218;
        int A_219_CUSTOM_RUN_9_ID = 219;
        int A_220_CUSTOM_RUN_10_ID = 220;
        int A_221_CUSTOM_JUMP_1_ID = 221;
        int A_222_CUSTOM_JUMP_2_ID = 222;
        int A_223_CUSTOM_JUMP_3_ID = 223;
        int A_224_CUSTOM_JUMP_4_ID = 224;
        int A_225_CUSTOM_JUMP_5_ID = 225;
        int A_226_CUSTOM_JUMP_6_ID = 226;
        int A_227_CUSTOM_JUMP_7_ID = 227;
        int A_228_CUSTOM_JUMP_8_ID = 228;
        int A_229_CUSTOM_JUMP_9_ID = 229;
        int A_230_CUSTOM_JUMP_10_ID = 230;
        int A_231_CUSTOM_JUMP_SPIN_1_ID = 231;
        int A_232_CUSTOM_JUMP_SPIN_2_ID = 232;
        int A_233_CUSTOM_JUMP_SPIN_3_ID = 233;
        int A_234_CUSTOM_JUMP_SPIN_4_ID = 234;
        int A_235_CUSTOM_JUMP_SPIN_5_ID = 235;
        int A_236_CUSTOM_JUMP_SPIN_6_ID = 236;
        int A_237_CUSTOM_JUMP_SPIN_7_ID = 237;
        int A_238_CUSTOM_JUMP_SPIN_8_ID = 238;
        int A_239_CUSTOM_JUMP_SPIN_9_ID = 239;
        int A_240_CUSTOM_JUMP_SPIN_10_ID = 240;
        int A_241_CUSTOM_FALL_1_ID = 241;
        int A_242_CUSTOM_FALL_2_ID = 242;
        int A_243_CUSTOM_FALL_3_ID = 243;
        int A_244_CUSTOM_FALL_4_ID = 244;
        int A_245_CUSTOM_FALL_5_ID = 245;
        int A_246_CUSTOM_FALL_6_ID = 246;
        int A_247_CUSTOM_FALL_7_ID = 247;
        int A_248_CUSTOM_FALL_8_ID = 248;
        int A_249_CUSTOM_FALL_9_ID = 249;
        int A_250_CUSTOM_FALL_10_ID = 250;
        int A_251_CUSTOM_LAND_1_ID = 251;
        int A_252_CUSTOM_LAND_2_ID = 252;
        int A_253_CUSTOM_LAND_3_ID = 253;
        int A_254_CUSTOM_LAND_4_ID = 254;
        int A_255_CUSTOM_LAND_5_ID = 255;
        int A_256_CUSTOM_LAND_6_ID = 256;
        int A_257_CUSTOM_LAND_7_ID = 257;
        int A_258_CUSTOM_LAND_8_ID = 258;
        int A_259_CUSTOM_LAND_9_ID = 259;
        int A_260_CUSTOM_LAND_10_ID = 260;
        int A_261_CUSTOM_PUNCH_1_ID = 261;
        int A_262_CUSTOM_PUNCH_2_ID = 262;
        int A_263_CUSTOM_PUNCH_3_ID = 263;
        int A_264_CUSTOM_PUNCH_4_ID = 264;
        int A_265_CUSTOM_PUNCH_5_ID = 265;
        int A_266_CUSTOM_PUNCH_6_ID = 266;
        int A_267_CUSTOM_PUNCH_7_ID = 267;
        int A_268_CUSTOM_PUNCH_8_ID = 268;
        int A_269_CUSTOM_PUNCH_9_ID = 269;
        int A_270_CUSTOM_PUNCH_10_ID = 270;
        int A_271_CUSTOM_KICK_1_ID = 271;
        int A_272_CUSTOM_KICK_2_ID = 272;
        int A_273_CUSTOM_KICK_3_ID = 273;
        int A_274_CUSTOM_KICK_4_ID = 274;
        int A_275_CUSTOM_KICK_5_ID = 275;
        int A_276_CUSTOM_KICK_6_ID = 276;
        int A_277_CUSTOM_KICK_7_ID = 277;
        int A_278_CUSTOM_KICK_8_ID = 278;
        int A_279_CUSTOM_KICK_9_ID = 279;
        int A_280_CUSTOM_KICK_10_ID = 280;
        int A_281_CUSTOM_ATTACK_1_ID = 281;
        int A_282_CUSTOM_ATTACK_2_ID = 282;
        int A_283_CUSTOM_ATTACK_3_ID = 283;
        int A_284_CUSTOM_ATTACK_4_ID = 284;
        int A_285_CUSTOM_ATTACK_5_ID = 285;
        int A_286_CUSTOM_ATTACK_6_ID = 286;
        int A_287_CUSTOM_ATTACK_7_ID = 287;
        int A_288_CUSTOM_ATTACK_8_ID = 288;
        int A_289_CUSTOM_ATTACK_9_ID = 289;
        int A_290_CUSTOM_ATTACK_10_ID = 290;
        int A_291_CUSTOM_MAGIC_1_ID = 291;
        int A_292_CUSTOM_MAGIC_2_ID = 292;
        int A_293_CUSTOM_MAGIC_3_ID = 293;
        int A_294_CUSTOM_MAGIC_4_ID = 294;
        int A_295_CUSTOM_MAGIC_5_ID = 295;
        int A_296_CUSTOM_MAGIC_6_ID = 296;
        int A_297_CUSTOM_MAGIC_7_ID = 297;
        int A_298_CUSTOM_MAGIC_8_ID = 298;
        int A_299_CUSTOM_MAGIC_9_ID = 299;
        int A_300_CUSTOM_MAGIC_10_ID = 300;
        int A_301_CUSTOM_BLOCK_1_ID = 301;
        int A_302_CUSTOM_BLOCK_2_ID = 302;
        int A_303_CUSTOM_BLOCK_3_ID = 303;
        int A_304_CUSTOM_BLOCK_4_ID = 304;
        int A_305_CUSTOM_BLOCK_5_ID = 305;
        int A_306_CUSTOM_BLOCK_6_ID = 306;
        int A_307_CUSTOM_BLOCK_7_ID = 307;
        int A_308_CUSTOM_BLOCK_8_ID = 308;
        int A_309_CUSTOM_BLOCK_9_ID = 309;
        int A_310_CUSTOM_BLOCK_10_ID = 310;
        int A_311_CUSTOM_HIT_1_ID = 311;
        int A_312_CUSTOM_HIT_2_ID = 312;
        int A_313_CUSTOM_HIT_3_ID = 313;
        int A_314_CUSTOM_HIT_4_ID = 314;
        int A_315_CUSTOM_HIT_5_ID = 315;
        int A_316_CUSTOM_HIT_6_ID = 316;
        int A_317_CUSTOM_HIT_7_ID = 317;
        int A_318_CUSTOM_HIT_8_ID = 318;
        int A_319_CUSTOM_HIT_9_ID = 319;
        int A_320_CUSTOM_HIT_10_ID = 320;
        int A_321_CUSTOM_LOSE_1_ID = 321;
        int A_322_CUSTOM_LOSE_2_ID = 322;
        int A_323_CUSTOM_LOSE_3_ID = 323;
        int A_324_CUSTOM_LOSE_4_ID = 324;
        int A_325_CUSTOM_LOSE_5_ID = 325;
        int A_326_CUSTOM_LOSE_6_ID = 326;
        int A_327_CUSTOM_LOSE_7_ID = 327;
        int A_328_CUSTOM_LOSE_8_ID = 328;
        int A_329_CUSTOM_LOSE_9_ID = 329;
        int A_330_CUSTOM_LOSE_10_ID = 330;
        int A_331_CUSTOM_DIE_1_ID = 331;
        int A_332_CUSTOM_DIE_2_ID = 332;
        int A_333_CUSTOM_DIE_3_ID = 333;
        int A_334_CUSTOM_DIE_4_ID = 334;
        int A_335_CUSTOM_DIE_5_ID = 335;
        int A_336_CUSTOM_DIE_6_ID = 336;
        int A_337_CUSTOM_DIE_7_ID = 337;
        int A_338_CUSTOM_DIE_8_ID = 338;
        int A_339_CUSTOM_DIE_9_ID = 339;
        int A_340_CUSTOM_DIE_10_ID = 340;
        int A_341_CUSTOM_VICTORY_1_ID = 341;
        int A_342_CUSTOM_VICTORY_2_ID = 342;
        int A_343_CUSTOM_VICTORY_3_ID = 343;
        int A_344_CUSTOM_VICTORY_4_ID = 344;
        int A_345_CUSTOM_VICTORY_5_ID = 345;
        int A_346_CUSTOM_VICTORY_6_ID = 346;
        int A_347_CUSTOM_VICTORY_7_ID = 347;
        int A_348_CUSTOM_VICTORY_8_ID = 348;
        int A_349_CUSTOM_VICTORY_9_ID = 349;
        int A_350_CUSTOM_VICTORY_10_ID = 350;
        int A_351_CUSTOM_EXTRA_ACTIONS_1_ID = 351;
        int A_352_CUSTOM_EXTRA_ACTIONS_2_ID = 352;
        int A_353_CUSTOM_EXTRA_ACTIONS_3_ID = 353;
        int A_354_CUSTOM_EXTRA_ACTIONS_4_ID = 354;
        int A_355_CUSTOM_EXTRA_ACTIONS_5_ID = 355;
        int A_356_CUSTOM_EXTRA_ACTIONS_6_ID = 356;
        int A_357_CUSTOM_EXTRA_ACTIONS_7_ID = 357;
        int A_358_CUSTOM_EXTRA_ACTIONS_8_ID = 358;
        int A_359_CUSTOM_EXTRA_ACTIONS_9_ID = 359;
        int A_360_CUSTOM_EXTRA_ACTIONS_10_ID = 360;
        string backActionName = "011_idle_1";
        int backActionID = 11;
        private void Awake()
        {
            gameObject.transform.position = new Vector3(-57, 5, -45);
            FindComponents();
            actions[A_001_POSE_1] = A_001_POSE_1_ID;
            actions[A_002_POSE_2] = A_002_POSE_2_ID;
            actions[A_003_POSE_3] = A_003_POSE_3_ID;
            actions[A_004_POSE_4] = A_004_POSE_4_ID;
            actions[A_005_POSE_5] = A_005_POSE_5_ID;
            actions[A_006_POSE_6] = A_006_POSE_6_ID;
            actions[A_007_POSE_7] = A_007_POSE_7_ID;
            actions[A_008_POSE_8] = A_008_POSE_8_ID;
            actions[A_009_POSE_9] = A_009_POSE_9_ID;
            actions[A_010_POSE_10] = A_010_POSE_10_ID;
            actions[A_011_IDLE_1] = A_011_IDLE_1_ID;
            actions[A_012_IDLE_2] = A_012_IDLE_2_ID;
            actions[A_013_IDLE_3] = A_013_IDLE_3_ID;
            actions[A_014_IDLE_4] = A_014_IDLE_4_ID;
            actions[A_015_IDLE_5] = A_015_IDLE_5_ID;
            actions[A_016_IDLE_6] = A_016_IDLE_6_ID;
            actions[A_017_IDLE_7] = A_017_IDLE_7_ID;
            actions[A_018_IDLE_8] = A_018_IDLE_8_ID;
            actions[A_019_IDLE_9] = A_019_IDLE_9_ID;
            actions[A_020_IDLE_10] = A_020_IDLE_10_ID;
            actions[A_021_WALK_1] = A_021_WALK_1_ID;
            actions[A_022_WALK_2] = A_022_WALK_2_ID;
            actions[A_023_WALK_3] = A_023_WALK_3_ID;
            actions[A_024_WALK_4] = A_024_WALK_4_ID;
            actions[A_025_WALK_5] = A_025_WALK_5_ID;
            actions[A_026_WALK_6] = A_026_WALK_6_ID;
            actions[A_027_WALK_7] = A_027_WALK_7_ID;
            actions[A_028_WALK_8] = A_028_WALK_8_ID;
            actions[A_029_WALK_9] = A_029_WALK_9_ID;
            actions[A_030_WALK_10] = A_030_WALK_10_ID;
            actions[A_031_RUN_1] = A_031_RUN_1_ID;
            actions[A_032_RUN_2] = A_032_RUN_2_ID;
            actions[A_033_RUN_3] = A_033_RUN_3_ID;
            actions[A_034_RUN_4] = A_034_RUN_4_ID;
            actions[A_035_RUN_5] = A_035_RUN_5_ID;
            actions[A_036_RUN_6] = A_036_RUN_6_ID;
            actions[A_037_RUN_7] = A_037_RUN_7_ID;
            actions[A_038_RUN_8] = A_038_RUN_8_ID;
            actions[A_039_RUN_9] = A_039_RUN_9_ID;
            actions[A_040_RUN_10] = A_040_RUN_10_ID;
            actions[A_041_JUMP_1] = A_041_JUMP_1_ID;
            actions[A_042_JUMP_2] = A_042_JUMP_2_ID;
            actions[A_043_JUMP_3] = A_043_JUMP_3_ID;
            actions[A_044_JUMP_4] = A_044_JUMP_4_ID;
            actions[A_045_JUMP_5] = A_045_JUMP_5_ID;
            actions[A_046_JUMP_6] = A_046_JUMP_6_ID;
            actions[A_047_JUMP_7] = A_047_JUMP_7_ID;
            actions[A_048_JUMP_8] = A_048_JUMP_8_ID;
            actions[A_049_JUMP_9] = A_049_JUMP_9_ID;
            actions[A_050_JUMP_10] = A_050_JUMP_10_ID;
            actions[A_051_JUMP_SPIN_1] = A_051_JUMP_SPIN_1_ID;
            actions[A_052_JUMP_SPIN_2] = A_052_JUMP_SPIN_2_ID;
            actions[A_053_JUMP_SPIN_3] = A_053_JUMP_SPIN_3_ID;
            actions[A_054_JUMP_SPIN_4] = A_054_JUMP_SPIN_4_ID;
            actions[A_055_JUMP_SPIN_5] = A_055_JUMP_SPIN_5_ID;
            actions[A_056_JUMP_SPIN_6] = A_056_JUMP_SPIN_6_ID;
            actions[A_057_JUMP_SPIN_7] = A_057_JUMP_SPIN_7_ID;
            actions[A_058_JUMP_SPIN_8] = A_058_JUMP_SPIN_8_ID;
            actions[A_059_JUMP_SPIN_9] = A_059_JUMP_SPIN_9_ID;
            actions[A_060_JUMP_SPIN_10] = A_060_JUMP_SPIN_10_ID;
            actions[A_061_FALL_1] = A_061_FALL_1_ID;
            actions[A_062_FALL_2] = A_062_FALL_2_ID;
            actions[A_063_FALL_3] = A_063_FALL_3_ID;
            actions[A_064_FALL_4] = A_064_FALL_4_ID;
            actions[A_065_FALL_5] = A_065_FALL_5_ID;
            actions[A_066_FALL_6] = A_066_FALL_6_ID;
            actions[A_067_FALL_7] = A_067_FALL_7_ID;
            actions[A_068_FALL_8] = A_068_FALL_8_ID;
            actions[A_069_FALL_9] = A_069_FALL_9_ID;
            actions[A_070_FALL_10] = A_070_FALL_10_ID;
            actions[A_071_LAND_1] = A_071_LAND_1_ID;
            actions[A_072_LAND_2] = A_072_LAND_2_ID;
            actions[A_073_LAND_3] = A_073_LAND_3_ID;
            actions[A_074_LAND_4] = A_074_LAND_4_ID;
            actions[A_075_LAND_5] = A_075_LAND_5_ID;
            actions[A_076_LAND_6] = A_076_LAND_6_ID;
            actions[A_077_LAND_7] = A_077_LAND_7_ID;
            actions[A_078_LAND_8] = A_078_LAND_8_ID;
            actions[A_079_LAND_9] = A_079_LAND_9_ID;
            actions[A_080_LAND_10] = A_080_LAND_10_ID;
            actions[A_081_PUNCH_1] = A_081_PUNCH_1_ID;
            actions[A_082_PUNCH_2] = A_082_PUNCH_2_ID;
            actions[A_083_PUNCH_3] = A_083_PUNCH_3_ID;
            actions[A_084_PUNCH_4] = A_084_PUNCH_4_ID;
            actions[A_085_PUNCH_5] = A_085_PUNCH_5_ID;
            actions[A_086_PUNCH_6] = A_086_PUNCH_6_ID;
            actions[A_087_PUNCH_7] = A_087_PUNCH_7_ID;
            actions[A_088_PUNCH_8] = A_088_PUNCH_8_ID;
            actions[A_089_PUNCH_9] = A_089_PUNCH_9_ID;
            actions[A_090_PUNCH_10] = A_090_PUNCH_10_ID;
            actions[A_091_KICK_1] = A_091_KICK_1_ID;
            actions[A_092_KICK_2] = A_092_KICK_2_ID;
            actions[A_093_KICK_3] = A_093_KICK_3_ID;
            actions[A_094_KICK_4] = A_094_KICK_4_ID;
            actions[A_095_KICK_5] = A_095_KICK_5_ID;
            actions[A_096_KICK_6] = A_096_KICK_6_ID;
            actions[A_097_KICK_7] = A_097_KICK_7_ID;
            actions[A_098_KICK_8] = A_098_KICK_8_ID;
            actions[A_099_KICK_9] = A_099_KICK_9_ID;
            actions[A_100_KICK_10] = A_100_KICK_10_ID;
            actions[A_101_ATTACK_1] = A_101_ATTACK_1_ID;
            actions[A_102_ATTACK_2] = A_102_ATTACK_2_ID;
            actions[A_103_ATTACK_3] = A_103_ATTACK_3_ID;
            actions[A_104_ATTACK_4] = A_104_ATTACK_4_ID;
            actions[A_105_ATTACK_5] = A_105_ATTACK_5_ID;
            actions[A_106_ATTACK_6] = A_106_ATTACK_6_ID;
            actions[A_107_ATTACK_7] = A_107_ATTACK_7_ID;
            actions[A_108_ATTACK_8] = A_108_ATTACK_8_ID;
            actions[A_109_ATTACK_9] = A_109_ATTACK_9_ID;
            actions[A_110_ATTACK_10] = A_110_ATTACK_10_ID;
            actions[A_111_MAGIC_1] = A_111_MAGIC_1_ID;
            actions[A_112_MAGIC_2] = A_112_MAGIC_2_ID;
            actions[A_113_MAGIC_3] = A_113_MAGIC_3_ID;
            actions[A_114_MAGIC_4] = A_114_MAGIC_4_ID;
            actions[A_115_MAGIC_5] = A_115_MAGIC_5_ID;
            actions[A_116_MAGIC_6] = A_116_MAGIC_6_ID;
            actions[A_117_MAGIC_7] = A_117_MAGIC_7_ID;
            actions[A_118_MAGIC_8] = A_118_MAGIC_8_ID;
            actions[A_119_MAGIC_9] = A_119_MAGIC_9_ID;
            actions[A_120_MAGIC_10] = A_120_MAGIC_10_ID;
            actions[A_121_BLOCK_1] = A_121_BLOCK_1_ID;
            actions[A_122_BLOCK_2] = A_122_BLOCK_2_ID;
            actions[A_123_BLOCK_3] = A_123_BLOCK_3_ID;
            actions[A_124_BLOCK_4] = A_124_BLOCK_4_ID;
            actions[A_125_BLOCK_5] = A_125_BLOCK_5_ID;
            actions[A_126_BLOCK_6] = A_126_BLOCK_6_ID;
            actions[A_127_BLOCK_7] = A_127_BLOCK_7_ID;
            actions[A_128_BLOCK_8] = A_128_BLOCK_8_ID;
            actions[A_129_BLOCK_9] = A_129_BLOCK_9_ID;
            actions[A_130_BLOCK_10] = A_130_BLOCK_10_ID;
            actions[A_131_HIT_1] = A_131_HIT_1_ID;
            actions[A_132_HIT_2] = A_132_HIT_2_ID;
            actions[A_133_HIT_3] = A_133_HIT_3_ID;
            actions[A_134_HIT_4] = A_134_HIT_4_ID;
            actions[A_135_HIT_5] = A_135_HIT_5_ID;
            actions[A_136_HIT_6] = A_136_HIT_6_ID;
            actions[A_137_HIT_7] = A_137_HIT_7_ID;
            actions[A_138_HIT_8] = A_138_HIT_8_ID;
            actions[A_139_HIT_9] = A_139_HIT_9_ID;
            actions[A_140_HIT_10] = A_140_HIT_10_ID;
            actions[A_141_LOSE_1] = A_141_LOSE_1_ID;
            actions[A_142_LOSE_2] = A_142_LOSE_2_ID;
            actions[A_143_LOSE_3] = A_143_LOSE_3_ID;
            actions[A_144_LOSE_4] = A_144_LOSE_4_ID;
            actions[A_145_LOSE_5] = A_145_LOSE_5_ID;
            actions[A_146_LOSE_6] = A_146_LOSE_6_ID;
            actions[A_147_LOSE_7] = A_147_LOSE_7_ID;
            actions[A_148_LOSE_8] = A_148_LOSE_8_ID;
            actions[A_149_LOSE_9] = A_149_LOSE_9_ID;
            actions[A_150_LOSE_10] = A_150_LOSE_10_ID;
            actions[A_151_DIE_1] = A_151_DIE_1_ID;
            actions[A_152_DIE_2] = A_152_DIE_2_ID;
            actions[A_153_DIE_3] = A_153_DIE_3_ID;
            actions[A_154_DIE_4] = A_154_DIE_4_ID;
            actions[A_155_DIE_5] = A_155_DIE_5_ID;
            actions[A_156_DIE_6] = A_156_DIE_6_ID;
            actions[A_157_DIE_7] = A_157_DIE_7_ID;
            actions[A_158_DIE_8] = A_158_DIE_8_ID;
            actions[A_159_DIE_9] = A_159_DIE_9_ID;
            actions[A_160_DIE_10] = A_160_DIE_10_ID;
            actions[A_161_VICTORY_1] = A_161_VICTORY_1_ID;
            actions[A_162_VICTORY_2] = A_162_VICTORY_2_ID;
            actions[A_163_VICTORY_3] = A_163_VICTORY_3_ID;
            actions[A_164_VICTORY_4] = A_164_VICTORY_4_ID;
            actions[A_165_VICTORY_5] = A_165_VICTORY_5_ID;
            actions[A_166_VICTORY_6] = A_166_VICTORY_6_ID;
            actions[A_167_VICTORY_7] = A_167_VICTORY_7_ID;
            actions[A_168_VICTORY_8] = A_168_VICTORY_8_ID;
            actions[A_169_VICTORY_9] = A_169_VICTORY_9_ID;
            actions[A_170_VICTORY_10] = A_170_VICTORY_10_ID;
            actions[A_171_EXTRA_ACTIONS_1] = A_171_EXTRA_ACTIONS_1_ID;
            actions[A_172_EXTRA_ACTIONS_2] = A_172_EXTRA_ACTIONS_2_ID;
            actions[A_173_EXTRA_ACTIONS_3] = A_173_EXTRA_ACTIONS_3_ID;
            actions[A_174_EXTRA_ACTIONS_4] = A_174_EXTRA_ACTIONS_4_ID;
            actions[A_175_EXTRA_ACTIONS_5] = A_175_EXTRA_ACTIONS_5_ID;
            actions[A_176_EXTRA_ACTIONS_6] = A_176_EXTRA_ACTIONS_6_ID;
            actions[A_177_EXTRA_ACTIONS_7] = A_177_EXTRA_ACTIONS_7_ID;
            actions[A_178_EXTRA_ACTIONS_8] = A_178_EXTRA_ACTIONS_8_ID;
            actions[A_179_EXTRA_ACTIONS_9] = A_179_EXTRA_ACTIONS_9_ID;
            actions[A_180_EXTRA_ACTIONS_10] = A_180_EXTRA_ACTIONS_10_ID;
            actions[A_181_CUSTOM_POSE_1] = A_181_CUSTOM_POSE_1_ID;
            actions[A_182_CUSTOM_POSE_2] = A_182_CUSTOM_POSE_2_ID;
            actions[A_183_CUSTOM_POSE_3] = A_183_CUSTOM_POSE_3_ID;
            actions[A_184_CUSTOM_POSE_4] = A_184_CUSTOM_POSE_4_ID;
            actions[A_185_CUSTOM_POSE_5] = A_185_CUSTOM_POSE_5_ID;
            actions[A_186_CUSTOM_POSE_6] = A_186_CUSTOM_POSE_6_ID;
            actions[A_187_CUSTOM_POSE_7] = A_187_CUSTOM_POSE_7_ID;
            actions[A_188_CUSTOM_POSE_8] = A_188_CUSTOM_POSE_8_ID;
            actions[A_189_CUSTOM_POSE_9] = A_189_CUSTOM_POSE_9_ID;
            actions[A_190_CUSTOM_POSE_10] = A_190_CUSTOM_POSE_10_ID;
            actions[A_191_CUSTOM_IDLE_1] = A_191_CUSTOM_IDLE_1_ID;
            actions[A_192_CUSTOM_IDLE_2] = A_192_CUSTOM_IDLE_2_ID;
            actions[A_193_CUSTOM_IDLE_3] = A_193_CUSTOM_IDLE_3_ID;
            actions[A_194_CUSTOM_IDLE_4] = A_194_CUSTOM_IDLE_4_ID;
            actions[A_195_CUSTOM_IDLE_5] = A_195_CUSTOM_IDLE_5_ID;
            actions[A_196_CUSTOM_IDLE_6] = A_196_CUSTOM_IDLE_6_ID;
            actions[A_197_CUSTOM_IDLE_7] = A_197_CUSTOM_IDLE_7_ID;
            actions[A_198_CUSTOM_IDLE_8] = A_198_CUSTOM_IDLE_8_ID;
            actions[A_199_CUSTOM_IDLE_9] = A_199_CUSTOM_IDLE_9_ID;
            actions[A_200_CUSTOM_IDLE_10] = A_200_CUSTOM_IDLE_10_ID;
            actions[A_201_CUSTOM_WALK_1] = A_201_CUSTOM_WALK_1_ID;
            actions[A_202_CUSTOM_WALK_2] = A_202_CUSTOM_WALK_2_ID;
            actions[A_203_CUSTOM_WALK_3] = A_203_CUSTOM_WALK_3_ID;
            actions[A_204_CUSTOM_WALK_4] = A_204_CUSTOM_WALK_4_ID;
            actions[A_205_CUSTOM_WALK_5] = A_205_CUSTOM_WALK_5_ID;
            actions[A_206_CUSTOM_WALK_6] = A_206_CUSTOM_WALK_6_ID;
            actions[A_207_CUSTOM_WALK_7] = A_207_CUSTOM_WALK_7_ID;
            actions[A_208_CUSTOM_WALK_8] = A_208_CUSTOM_WALK_8_ID;
            actions[A_209_CUSTOM_WALK_9] = A_209_CUSTOM_WALK_9_ID;
            actions[A_210_CUSTOM_WALK_10] = A_210_CUSTOM_WALK_10_ID;
            actions[A_211_CUSTOM_RUN_1] = A_211_CUSTOM_RUN_1_ID;
            actions[A_212_CUSTOM_RUN_2] = A_212_CUSTOM_RUN_2_ID;
            actions[A_213_CUSTOM_RUN_3] = A_213_CUSTOM_RUN_3_ID;
            actions[A_214_CUSTOM_RUN_4] = A_214_CUSTOM_RUN_4_ID;
            actions[A_215_CUSTOM_RUN_5] = A_215_CUSTOM_RUN_5_ID;
            actions[A_216_CUSTOM_RUN_6] = A_216_CUSTOM_RUN_6_ID;
            actions[A_217_CUSTOM_RUN_7] = A_217_CUSTOM_RUN_7_ID;
            actions[A_218_CUSTOM_RUN_8] = A_218_CUSTOM_RUN_8_ID;
            actions[A_219_CUSTOM_RUN_9] = A_219_CUSTOM_RUN_9_ID;
            actions[A_220_CUSTOM_RUN_10] = A_220_CUSTOM_RUN_10_ID;
            actions[A_221_CUSTOM_JUMP_1] = A_221_CUSTOM_JUMP_1_ID;
            actions[A_222_CUSTOM_JUMP_2] = A_222_CUSTOM_JUMP_2_ID;
            actions[A_223_CUSTOM_JUMP_3] = A_223_CUSTOM_JUMP_3_ID;
            actions[A_224_CUSTOM_JUMP_4] = A_224_CUSTOM_JUMP_4_ID;
            actions[A_225_CUSTOM_JUMP_5] = A_225_CUSTOM_JUMP_5_ID;
            actions[A_226_CUSTOM_JUMP_6] = A_226_CUSTOM_JUMP_6_ID;
            actions[A_227_CUSTOM_JUMP_7] = A_227_CUSTOM_JUMP_7_ID;
            actions[A_228_CUSTOM_JUMP_8] = A_228_CUSTOM_JUMP_8_ID;
            actions[A_229_CUSTOM_JUMP_9] = A_229_CUSTOM_JUMP_9_ID;
            actions[A_230_CUSTOM_JUMP_10] = A_230_CUSTOM_JUMP_10_ID;
            actions[A_231_CUSTOM_JUMP_SPIN_1] = A_231_CUSTOM_JUMP_SPIN_1_ID;
            actions[A_232_CUSTOM_JUMP_SPIN_2] = A_232_CUSTOM_JUMP_SPIN_2_ID;
            actions[A_233_CUSTOM_JUMP_SPIN_3] = A_233_CUSTOM_JUMP_SPIN_3_ID;
            actions[A_234_CUSTOM_JUMP_SPIN_4] = A_234_CUSTOM_JUMP_SPIN_4_ID;
            actions[A_235_CUSTOM_JUMP_SPIN_5] = A_235_CUSTOM_JUMP_SPIN_5_ID;
            actions[A_236_CUSTOM_JUMP_SPIN_6] = A_236_CUSTOM_JUMP_SPIN_6_ID;
            actions[A_237_CUSTOM_JUMP_SPIN_7] = A_237_CUSTOM_JUMP_SPIN_7_ID;
            actions[A_238_CUSTOM_JUMP_SPIN_8] = A_238_CUSTOM_JUMP_SPIN_8_ID;
            actions[A_239_CUSTOM_JUMP_SPIN_9] = A_239_CUSTOM_JUMP_SPIN_9_ID;
            actions[A_240_CUSTOM_JUMP_SPIN_10] = A_240_CUSTOM_JUMP_SPIN_10_ID;
            actions[A_241_CUSTOM_FALL_1] = A_241_CUSTOM_FALL_1_ID;
            actions[A_242_CUSTOM_FALL_2] = A_242_CUSTOM_FALL_2_ID;
            actions[A_243_CUSTOM_FALL_3] = A_243_CUSTOM_FALL_3_ID;
            actions[A_244_CUSTOM_FALL_4] = A_244_CUSTOM_FALL_4_ID;
            actions[A_245_CUSTOM_FALL_5] = A_245_CUSTOM_FALL_5_ID;
            actions[A_246_CUSTOM_FALL_6] = A_246_CUSTOM_FALL_6_ID;
            actions[A_247_CUSTOM_FALL_7] = A_247_CUSTOM_FALL_7_ID;
            actions[A_248_CUSTOM_FALL_8] = A_248_CUSTOM_FALL_8_ID;
            actions[A_249_CUSTOM_FALL_9] = A_249_CUSTOM_FALL_9_ID;
            actions[A_250_CUSTOM_FALL_10] = A_250_CUSTOM_FALL_10_ID;
            actions[A_251_CUSTOM_LAND_1] = A_251_CUSTOM_LAND_1_ID;
            actions[A_252_CUSTOM_LAND_2] = A_252_CUSTOM_LAND_2_ID;
            actions[A_253_CUSTOM_LAND_3] = A_253_CUSTOM_LAND_3_ID;
            actions[A_254_CUSTOM_LAND_4] = A_254_CUSTOM_LAND_4_ID;
            actions[A_255_CUSTOM_LAND_5] = A_255_CUSTOM_LAND_5_ID;
            actions[A_256_CUSTOM_LAND_6] = A_256_CUSTOM_LAND_6_ID;
            actions[A_257_CUSTOM_LAND_7] = A_257_CUSTOM_LAND_7_ID;
            actions[A_258_CUSTOM_LAND_8] = A_258_CUSTOM_LAND_8_ID;
            actions[A_259_CUSTOM_LAND_9] = A_259_CUSTOM_LAND_9_ID;
            actions[A_260_CUSTOM_LAND_10] = A_260_CUSTOM_LAND_10_ID;
            actions[A_261_CUSTOM_PUNCH_1] = A_261_CUSTOM_PUNCH_1_ID;
            actions[A_262_CUSTOM_PUNCH_2] = A_262_CUSTOM_PUNCH_2_ID;
            actions[A_263_CUSTOM_PUNCH_3] = A_263_CUSTOM_PUNCH_3_ID;
            actions[A_264_CUSTOM_PUNCH_4] = A_264_CUSTOM_PUNCH_4_ID;
            actions[A_265_CUSTOM_PUNCH_5] = A_265_CUSTOM_PUNCH_5_ID;
            actions[A_266_CUSTOM_PUNCH_6] = A_266_CUSTOM_PUNCH_6_ID;
            actions[A_267_CUSTOM_PUNCH_7] = A_267_CUSTOM_PUNCH_7_ID;
            actions[A_268_CUSTOM_PUNCH_8] = A_268_CUSTOM_PUNCH_8_ID;
            actions[A_269_CUSTOM_PUNCH_9] = A_269_CUSTOM_PUNCH_9_ID;
            actions[A_270_CUSTOM_PUNCH_10] = A_270_CUSTOM_PUNCH_10_ID;
            actions[A_271_CUSTOM_KICK_1] = A_271_CUSTOM_KICK_1_ID;
            actions[A_272_CUSTOM_KICK_2] = A_272_CUSTOM_KICK_2_ID;
            actions[A_273_CUSTOM_KICK_3] = A_273_CUSTOM_KICK_3_ID;
            actions[A_274_CUSTOM_KICK_4] = A_274_CUSTOM_KICK_4_ID;
            actions[A_275_CUSTOM_KICK_5] = A_275_CUSTOM_KICK_5_ID;
            actions[A_276_CUSTOM_KICK_6] = A_276_CUSTOM_KICK_6_ID;
            actions[A_277_CUSTOM_KICK_7] = A_277_CUSTOM_KICK_7_ID;
            actions[A_278_CUSTOM_KICK_8] = A_278_CUSTOM_KICK_8_ID;
            actions[A_279_CUSTOM_KICK_9] = A_279_CUSTOM_KICK_9_ID;
            actions[A_280_CUSTOM_KICK_10] = A_280_CUSTOM_KICK_10_ID;
            actions[A_281_CUSTOM_ATTACK_1] = A_281_CUSTOM_ATTACK_1_ID;
            actions[A_282_CUSTOM_ATTACK_2] = A_282_CUSTOM_ATTACK_2_ID;
            actions[A_283_CUSTOM_ATTACK_3] = A_283_CUSTOM_ATTACK_3_ID;
            actions[A_284_CUSTOM_ATTACK_4] = A_284_CUSTOM_ATTACK_4_ID;
            actions[A_285_CUSTOM_ATTACK_5] = A_285_CUSTOM_ATTACK_5_ID;
            actions[A_286_CUSTOM_ATTACK_6] = A_286_CUSTOM_ATTACK_6_ID;
            actions[A_287_CUSTOM_ATTACK_7] = A_287_CUSTOM_ATTACK_7_ID;
            actions[A_288_CUSTOM_ATTACK_8] = A_288_CUSTOM_ATTACK_8_ID;
            actions[A_289_CUSTOM_ATTACK_9] = A_289_CUSTOM_ATTACK_9_ID;
            actions[A_290_CUSTOM_ATTACK_10] = A_290_CUSTOM_ATTACK_10_ID;
            actions[A_291_CUSTOM_MAGIC_1] = A_291_CUSTOM_MAGIC_1_ID;
            actions[A_292_CUSTOM_MAGIC_2] = A_292_CUSTOM_MAGIC_2_ID;
            actions[A_293_CUSTOM_MAGIC_3] = A_293_CUSTOM_MAGIC_3_ID;
            actions[A_294_CUSTOM_MAGIC_4] = A_294_CUSTOM_MAGIC_4_ID;
            actions[A_295_CUSTOM_MAGIC_5] = A_295_CUSTOM_MAGIC_5_ID;
            actions[A_296_CUSTOM_MAGIC_6] = A_296_CUSTOM_MAGIC_6_ID;
            actions[A_297_CUSTOM_MAGIC_7] = A_297_CUSTOM_MAGIC_7_ID;
            actions[A_298_CUSTOM_MAGIC_8] = A_298_CUSTOM_MAGIC_8_ID;
            actions[A_299_CUSTOM_MAGIC_9] = A_299_CUSTOM_MAGIC_9_ID;
            actions[A_300_CUSTOM_MAGIC_10] = A_300_CUSTOM_MAGIC_10_ID;
            actions[A_301_CUSTOM_BLOCK_1] = A_301_CUSTOM_BLOCK_1_ID;
            actions[A_302_CUSTOM_BLOCK_2] = A_302_CUSTOM_BLOCK_2_ID;
            actions[A_303_CUSTOM_BLOCK_3] = A_303_CUSTOM_BLOCK_3_ID;
            actions[A_304_CUSTOM_BLOCK_4] = A_304_CUSTOM_BLOCK_4_ID;
            actions[A_305_CUSTOM_BLOCK_5] = A_305_CUSTOM_BLOCK_5_ID;
            actions[A_306_CUSTOM_BLOCK_6] = A_306_CUSTOM_BLOCK_6_ID;
            actions[A_307_CUSTOM_BLOCK_7] = A_307_CUSTOM_BLOCK_7_ID;
            actions[A_308_CUSTOM_BLOCK_8] = A_308_CUSTOM_BLOCK_8_ID;
            actions[A_309_CUSTOM_BLOCK_9] = A_309_CUSTOM_BLOCK_9_ID;
            actions[A_310_CUSTOM_BLOCK_10] = A_310_CUSTOM_BLOCK_10_ID;
            actions[A_311_CUSTOM_HIT_1] = A_311_CUSTOM_HIT_1_ID;
            actions[A_312_CUSTOM_HIT_2] = A_312_CUSTOM_HIT_2_ID;
            actions[A_313_CUSTOM_HIT_3] = A_313_CUSTOM_HIT_3_ID;
            actions[A_314_CUSTOM_HIT_4] = A_314_CUSTOM_HIT_4_ID;
            actions[A_315_CUSTOM_HIT_5] = A_315_CUSTOM_HIT_5_ID;
            actions[A_316_CUSTOM_HIT_6] = A_316_CUSTOM_HIT_6_ID;
            actions[A_317_CUSTOM_HIT_7] = A_317_CUSTOM_HIT_7_ID;
            actions[A_318_CUSTOM_HIT_8] = A_318_CUSTOM_HIT_8_ID;
            actions[A_319_CUSTOM_HIT_9] = A_319_CUSTOM_HIT_9_ID;
            actions[A_320_CUSTOM_HIT_10] = A_320_CUSTOM_HIT_10_ID;
            actions[A_321_CUSTOM_LOSE_1] = A_321_CUSTOM_LOSE_1_ID;
            actions[A_322_CUSTOM_LOSE_2] = A_322_CUSTOM_LOSE_2_ID;
            actions[A_323_CUSTOM_LOSE_3] = A_323_CUSTOM_LOSE_3_ID;
            actions[A_324_CUSTOM_LOSE_4] = A_324_CUSTOM_LOSE_4_ID;
            actions[A_325_CUSTOM_LOSE_5] = A_325_CUSTOM_LOSE_5_ID;
            actions[A_326_CUSTOM_LOSE_6] = A_326_CUSTOM_LOSE_6_ID;
            actions[A_327_CUSTOM_LOSE_7] = A_327_CUSTOM_LOSE_7_ID;
            actions[A_328_CUSTOM_LOSE_8] = A_328_CUSTOM_LOSE_8_ID;
            actions[A_329_CUSTOM_LOSE_9] = A_329_CUSTOM_LOSE_9_ID;
            actions[A_330_CUSTOM_LOSE_10] = A_330_CUSTOM_LOSE_10_ID;
            actions[A_331_CUSTOM_DIE_1] = A_331_CUSTOM_DIE_1_ID;
            actions[A_332_CUSTOM_DIE_2] = A_332_CUSTOM_DIE_2_ID;
            actions[A_333_CUSTOM_DIE_3] = A_333_CUSTOM_DIE_3_ID;
            actions[A_334_CUSTOM_DIE_4] = A_334_CUSTOM_DIE_4_ID;
            actions[A_335_CUSTOM_DIE_5] = A_335_CUSTOM_DIE_5_ID;
            actions[A_336_CUSTOM_DIE_6] = A_336_CUSTOM_DIE_6_ID;
            actions[A_337_CUSTOM_DIE_7] = A_337_CUSTOM_DIE_7_ID;
            actions[A_338_CUSTOM_DIE_8] = A_338_CUSTOM_DIE_8_ID;
            actions[A_339_CUSTOM_DIE_9] = A_339_CUSTOM_DIE_9_ID;
            actions[A_340_CUSTOM_DIE_10] = A_340_CUSTOM_DIE_10_ID;
            actions[A_341_CUSTOM_VICTORY_1] = A_341_CUSTOM_VICTORY_1_ID;
            actions[A_342_CUSTOM_VICTORY_2] = A_342_CUSTOM_VICTORY_2_ID;
            actions[A_343_CUSTOM_VICTORY_3] = A_343_CUSTOM_VICTORY_3_ID;
            actions[A_344_CUSTOM_VICTORY_4] = A_344_CUSTOM_VICTORY_4_ID;
            actions[A_345_CUSTOM_VICTORY_5] = A_345_CUSTOM_VICTORY_5_ID;
            actions[A_346_CUSTOM_VICTORY_6] = A_346_CUSTOM_VICTORY_6_ID;
            actions[A_347_CUSTOM_VICTORY_7] = A_347_CUSTOM_VICTORY_7_ID;
            actions[A_348_CUSTOM_VICTORY_8] = A_348_CUSTOM_VICTORY_8_ID;
            actions[A_349_CUSTOM_VICTORY_9] = A_349_CUSTOM_VICTORY_9_ID;
            actions[A_350_CUSTOM_VICTORY_10] = A_350_CUSTOM_VICTORY_10_ID;
            actions[A_351_CUSTOM_EXTRA_ACTIONS_1] = A_351_CUSTOM_EXTRA_ACTIONS_1_ID;
            actions[A_352_CUSTOM_EXTRA_ACTIONS_2] = A_352_CUSTOM_EXTRA_ACTIONS_2_ID;
            actions[A_353_CUSTOM_EXTRA_ACTIONS_3] = A_353_CUSTOM_EXTRA_ACTIONS_3_ID;
            actions[A_354_CUSTOM_EXTRA_ACTIONS_4] = A_354_CUSTOM_EXTRA_ACTIONS_4_ID;
            actions[A_355_CUSTOM_EXTRA_ACTIONS_5] = A_355_CUSTOM_EXTRA_ACTIONS_5_ID;
            actions[A_356_CUSTOM_EXTRA_ACTIONS_6] = A_356_CUSTOM_EXTRA_ACTIONS_6_ID;
            actions[A_357_CUSTOM_EXTRA_ACTIONS_7] = A_357_CUSTOM_EXTRA_ACTIONS_7_ID;
            actions[A_358_CUSTOM_EXTRA_ACTIONS_8] = A_358_CUSTOM_EXTRA_ACTIONS_8_ID;
            actions[A_359_CUSTOM_EXTRA_ACTIONS_9] = A_359_CUSTOM_EXTRA_ACTIONS_9_ID;
            actions[A_360_CUSTOM_EXTRA_ACTIONS_10] = A_360_CUSTOM_EXTRA_ACTIONS_10_ID;
            backActionName = A_011_IDLE_1;
            backActionID = A_011_IDLE_1_ID;
            UpdateAnimationAction();
        }



        void Start()
        {
            health = 1f;
            currentMoveAnimation = walkAnimation;
            UpdateAnimationAction();
            originalMoveSpeed = moveSpeed;
            originalScale = new Vector3(0.15f, 0.15f, 0.15f);
            originalJumpForce = jumpForce;
        }

        void Update()
        {
            // Debug.Log(health);

            UpdateAnimationAction();
            if (Input.GetMouseButton(0))
            {
                CharacterRotation();
            }
            CameraRotation();


            if (health < 1)
            {
                health += 0.0005f;
            }

            if (health < 0.2f)
            {
                EnableRunning = false;
                isRunning = false;
            }
            else
            {
                EnableRunning = true;
            }


            if (isRunning)
            {
                health -= 0.001f;
                if (currentMoveAnimation != runAnimation)
                {
                    moveSpeed = moveSpeed * 2;
                    currentMoveAnimation = runAnimation;
                    // theCamera.transform.Translate(0, 0, 10);

                    walkCamera.enabled = false;
                    runCamera.enabled = true;
                }
            }
            else
            {
                if (currentMoveAnimation != walkAnimation)
                {
                    moveSpeed = moveSpeed / 2;
                    currentMoveAnimation = walkAnimation;
                    // theCamera.transform.Translate(0, 0, -10);

                    walkCamera.enabled = true;
                    runCamera.enabled = false;
                }
            }

            Move();

            //-----직진
            if (Input.GetKeyDown("w"))
            {
                if (EnableRunning)
                {
                    if (Time.time - ClickStart < timeDoubleClick && Time.time - ClickStart > 0.1f)
                    {
                        if (!isRunning)
                        {
                            isRunning = true;
                        }
                    }
                    else
                    {
                        ClickStart = Time.time;
                    }
                }

            }

            if (Input.GetKey("w"))
            {
                isWalking = true;
                if (actionID != currentMoveAnimation[0] && !isFalling && !isWalking_LR)
                {
                    actionID = currentMoveAnimation[0];
                    SetActionInt(currentMoveAnimation[0]);

                }

            }
            else if (Input.GetKeyUp("w"))
            {
                isWalking = false;
                isRunning = false;
            }



            //-----뒤로
            if (Input.GetKey("s"))
            {
                isWalking = true;
                if (actionID != currentMoveAnimation[1] && !isFalling && !isWalking_LR)
                {
                    actionID = currentMoveAnimation[1];
                    SetActionInt(currentMoveAnimation[1]);

                }
            }
            else if (Input.GetKeyUp("s"))
            {
                isWalking = false;
            }




            //-----오른쪽
            if (Input.GetKey("d"))
            {
                isWalking = true;
                isWalking_LR = true;
                if (actionID != currentMoveAnimation[2] && !isFalling)
                {
                    actionID = currentMoveAnimation[2];
                    SetActionInt(currentMoveAnimation[2]);

                }
            }
            //------왼쪽
            if (Input.GetKey("a"))
            {
                isWalking = true;
                isWalking_LR = true;
                if (actionID != currentMoveAnimation[3] && !isFalling)
                {
                    actionID = currentMoveAnimation[3];
                    SetActionInt(currentMoveAnimation[3]);

                }
            }

            if (Input.GetKeyUp("d") || Input.GetKeyUp("a"))
            {
                isWalking = false;
                isWalking_LR = false;
            }


            if (!isWalking && !isFalling)
            {
                if (actionID != (int)actions[A_011_IDLE_1])
                {
                    actionID = (int)actions[A_011_IDLE_1];
                    SetActionInt(actionID);

                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            { //스페이스 키를 눌렀을 경우 -> 점프
                Jump();
            }

        }

        private void CharacterRotation()  // 좌우 캐릭터 회전
        {
            float _yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
            rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(_characterRotationY)); // 쿼터니언 * 쿼터니언																						// Debug.Log(myRigid.rotation);  // 쿼터니언																						// Debug.Log(myRigid.rotation.eulerAngles); // 벡터
        }
        private void CameraRotation()
        {
            float _xRotation = Input.GetAxisRaw("Mouse Y");
            float _cameraRotationX = _xRotation * lookSensitivity;

            currentCameraRotationX -= _cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            TheCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX + 15, 0f, 0f);

        }
        private void Move() // 캐릭터 움직이기
        {
            float _moveDirX = Input.GetAxisRaw("Horizontal");
            float _moveDirZ = Input.GetAxisRaw("Vertical");
            Vector3 _moveHorizontal = transform.right * _moveDirX;
            Vector3 _moveVertical = transform.forward * _moveDirZ;

            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * moveSpeed;

            rigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);
            minimapCamera.transform.position = transform.position + offset;
            popupCamera.transform.position = transform.position + offset;
        }


        public void FindComponents()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }

        //---------------점프
        public void Jump()
        {
            if (isGrounded == true)
            { //현재 캐릭터가 땅에 있을경우
              // rigidbody.velocity = new Vector3(0f, jumpForce, 0f);
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                isJumping = true;
                EnableDoubleJumping = true;
                isFalling = false;
            }
            else
            { //현재 캐릭터가 점프중일경우
                if (EnableDoubleJumping == true)
                { //현재 캐릭터가 더블 점프가 가능할경우
                    if ((isJumping == true) && isDoubleJumping == false)
                    { //점프중이고, 더블 점프중이 아닐 경우
                      // rigidbody.velocity = new Vector3(0f, jumpForce * 1.5f, 0f);
                        rigidbody.AddForce(Vector3.up * jumpForce * 1.2f, ForceMode.Impulse);
                        isDoubleJumping = true;
                        isFalling = false;
                    }
                }
            }
        }
        public Vector3 GetRigidBoy_Velocity()
        {
            return rigidbody.velocity;
        }
        public void ActionNoLoopedReturnToIdle(bool value)
        {
            actionNoLoopedReturnToIdle = value;
        }
        public void SetActionInt(int _actionID = -1)
        {
            ActionNoLoopedReturnToIdle(true);
            // if (_actionID == 61)
            // {
            // 	gameObject.transform.position = new Vector3(0, 2.5f, 0);
            // }
            StopCoroutine("ReturnToActionCoroutine");
            actionID = _actionID;
            FindComponents();
            animator.SetInteger("actionID", actionID);
            StopAllCoroutines();
            try
            {
                float waitTime = 1.0f;
                if (animator != null)
                {
                    animatorInfo = this.animator.GetCurrentAnimatorClipInfo(0);
                    for (int i = 0; i < animatorInfo.Length; i++)
                    {
                    }
                    if (animatorInfo.Length > 0)
                    {
                        currentAnimationClip = animatorInfo[0].clip;
                        if (currentAnimationClip != null)
                        {
                        }
                        currentAnimation = currentAnimationClip.name;
                        float clipDuration = currentAnimationClip.length;
                        float animatorSpeed = this.animator.speed;
                        waitTime = clipDuration / animatorSpeed;

                    }
                }
                StartCoroutine("WaitToActionCoroutine", waitTime);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
            }
            finally
            {
            }
            UpdateAnimationAction();
        }
        public void SetActionName(string _actionName = "011_idle_1")
        {
            StopCoroutine("ReturnToActionCoroutine");
            actionID = (int)actions[_actionName];
            animator.SetInteger("actionID", actionID);
            UpdateAnimationAction();
        }
        public void SetAnimatorSpeed(float _speed = 1)
        {
            animator.speed = _speed;
        }
        private void UpdateAnimationAction()
        {
            if (rigidbody == null)
            {
                Debug.LogWarning("rigidbody NOT FOUND!");
                return;
            }




            if (rigidbody.velocity.y > .1f)
            {
                if (actionID != (int)actions[A_041_JUMP_1] && isDoubleJumping == false)
                {
                    // Debug.Log("1");
                    actionID = (int)actions[A_041_JUMP_1];
                    SetActionInt(41);

                }
                if (actionID != (int)actions[A_051_JUMP_SPIN_1] && isDoubleJumping == true)
                {
                    // Debug.Log("2");
                    actionID = (int)actions[A_051_JUMP_SPIN_1];
                    SetActionInt(51);
                }

            }
            else if (rigidbody.velocity.y < -.1f && isFalling == false)
            {
                if (actionID != (int)actions[A_061_FALL_1])
                {
                    // Debug.Log("3");
                    SetActionInt(61);
                    isJumping = false;
                    isFalling = true;
                    isDoubleJumping = false;
                }
            }
            else if (rigidbody.velocity.y < 0.3f && rigidbody.velocity.y > -0.3f)
            {

                if (actionID != (int)actions[A_071_LAND_1] && actionID == (int)actions[A_061_FALL_1])
                {
                    // Debug.Log("4");
                    SetActionInt(71);
                    isJumping = false;
                    isFalling = false;
                    isDoubleJumping = false;
                }

                if (transform.position.y <= -10)
                {
                    // Debug.Log("5");
                    RestarLevel();
                }
            }
        }
        void ReturnToAction(string _actionName = "011_idle_1", float _returnTime = 2.0f)
        {
            backActionName = _actionName;
            backActionID = (int)actions[_actionName];
            StopCoroutine("ReturnToActionCoroutine");
            StartCoroutine("ReturnToActionCoroutine", _returnTime);
        }
        IEnumerator ReturnToActionCoroutine(float _returnTime = 3.0f)
        {
            yield return new WaitForSeconds(_returnTime);
            if (actionNoLoopedReturnToIdle == true)
            {
                if (backActionID != -1)
                {
                    SetActionInt(backActionID);
                }
                else if (backActionName != "")
                {
                    SetActionName(backActionName);
                }
            }
        }
        IEnumerator WaitToActionCoroutine(float _returnTime = 2.0f)
        {
            yield return new WaitForSeconds(_returnTime);
            try
            {
                bool hasLoop = true;
                float clipDuration = 1;
                float animatorSpeed = 1;
                if (animator != null)
                {
                    animatorInfo = this.animator.GetCurrentAnimatorClipInfo(0);
                    for (int i = 0; i < animatorInfo.Length; i++)
                    {
                    }
                    if (animatorInfo.Length > 0)
                    {
                        currentAnimationClip = animatorInfo[0].clip;
                        currentAnimation = currentAnimationClip.name;
                        clipDuration = currentAnimationClip.length;
                        animatorSpeed = this.animator.speed;
                        hasLoop = currentAnimationClip.isLooping;
                    }
                }
                if (hasLoop == true)
                {
                }
                else
                {
                    ReturnToAction(backActionName, clipDuration * animatorSpeed);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException("index parameter is out of range.", e);
            }
            finally
            {
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            //플레이어가 땅을 밟고 있을 때
            if (collision.transform.CompareTag("Ground"))
            {
                isGrounded = true;

                //점프에서 착지하는 과정
                if (actionID == (int)actions[A_071_LAND_1])
                {
                    SetActionName(A_071_LAND_1);
                    isFalling = false;
                    isJumping = false;
                    isDoubleJumping = false;
                    EnableDoubleJumping = false;
                    ReturnToAction(A_011_IDLE_1, 1.0f);
                }
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }

        public void RestarLevel()
        {
            Debug.Log("Restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "Item")// Shoe 아이템과 충돌했을 때
            {
                // 15초 동안 이동 속도를 변경
                moveSpeed = 10f;
                StartCoroutine(ResetMoveSpeedAfterDuration(15.0f)); // 15초 후 이동 속도를 복원
                Destroy(other.gameObject); // 충돌한 Shoe 아이템 제거
            }

            else if (other.name == "ShieldItem")
            {
                jumpForce = 6f;
                StartCoroutine(ResetJumpForceAfterDuration(15.0f));
                Destroy(other.gameObject);
                 

            }

            else if (other.name == "VoiceItem")// Shoe 아이템과 충돌했을 때
            {
                float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
                if (loudness < threshold)
                    loudness = 0;

                // Assuming you have a reference to the EnemyController script on the same GameObject
                EnemyController enemyController = GetComponent<EnemyController>();

                if (enemyController != null)
                {
                    // Set the IsChasing variable in the EnemyController script
                    enemyController.isChasing = loudness > 5;
                    transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

                    // 추가: 'VoiceItem'과 충돌 처리가 완료되면 'VoiceItem'을 비활성화
                    other.gameObject.SetActive(false);
                }
            }

        }

        IEnumerator ResetMoveSpeedAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            moveSpeed = originalMoveSpeed; // 원래 이동 속도로 복원
        }



IEnumerator ResetJumpForceAfterDuration(float duration)
{
    yield return new WaitForSeconds(duration);
    jumpForce = originalJumpForce; // 원래 jumpForce로 복원
}




    }




}