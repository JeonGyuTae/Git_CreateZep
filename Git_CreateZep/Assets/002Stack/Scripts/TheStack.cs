using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    // ��ü�� ũ�� ����
    private const float BoundSize = 3.5f;
    // ��ü�� �̵��ϴ� �� ����
    private const float MovingBoundsSize = 3f;
    // �ӷ� ����
    private const float StackMovingSpeed = 5.0f;
    // ��ü �ӷ� ����
    private const float BlockMovingSpeed = 3.5f;
    // ��� ������ �ּ� �� ���� (�� ������ ������ ���� ����)
    private const float ErrorMargin = 0.1f;

    // Prefab���� ������ ��ü ����
    public GameObject originBlock = null;

    // ������ ��ü ��ġ �� ����
    private Vector3 prevBlockPosition;
    // ��ü�� ���� ��ġ �� ���� <- ī�޶��� ������ ȭ�� ��ȯ�� ���� ���
    private Vector3 desiredPosition;
    // ������ ��ü�� ũ�� ����
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

    /*   �̵� ó�� ���� ���� ������ ������ �κ�   */
    // ���� ��ü �� ����
    Transform lastBlock = null;
    // ���� ��ġ �� ����
    float blockTransition = 0f;
    // �ֱ� ��ġ �� ����
    float secondaryPosition = 0f;

    // ������ ��ü�� ���� ���� => ���� ���� �� +1 �ϱ⿡ -1 �� ����
    int stackCount = -1;
    public int Score { get { return stackCount; } }
    // ������ ������ Ƚ�� ����
    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    private int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    // ���� ���� ����
    public Color prevColor;
    // ���� ���� ����
    public Color nextColor;

    // �̵� ���� üũ_X��
    bool isMovingX = false;

    int bestScore = 0;
    public int BestScore { get => bestScore; }

    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }

    private const string BestScoreKey = "BestScore";
    private const string BestComboKey = "BestCombo";

    void Start()
    {
        // Prefab���� ������ ��ü�� ã�� ������ ���
        if (originBlock == null)
        {
            // ���� �޼��� ���_OriginObject Not Found
            Debug.LogError("OriginBlock is Null");
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        // ���� ���� �� ����_���� ���� �޼��� ȣ��
        prevColor = GetRandomColor();
        // ���� ���� �� ����_���� ���� �޼��� ȣ��
        nextColor = GetRandomColor();

        // ���� ��ü�� ��ġ �� ����_Vector3.Down(= Y �� -1)
        prevBlockPosition = Vector3.down;

        // ��ü ���� �޼��� ȣ��
        Spawn_Block();
    }

    void Update()
    {
        // Player�� �Է� ��ȣ_���콺 ��Ŭ��(0)�� �۽����� ���
        if (Input.GetMouseButtonDown(0))
        {
            // ��ü ���� �޼��尡 ȣ��Ǿ��� ���
            if (PlaceBlock())
            {
                // ��ü ���� �޼��� ȣ��
                Spawn_Block();
            }
            // ��ü ���� �޼��尡 ȣ����� �ʾ��� ���
            else
            {
                // ���� ����
                Debug.Log("Game Over");

                UpdateScore();
            }
        }

        // ��ü �̵� �޼��� ȣ��
        MoveBlock();
        // ��ü�� ��ġ �� ����_�����ϰ�(��� ����, ���� ����, �̵� FPS % �� / ī�޶��� �ε巯�� ��ȯ�� ����)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    // ��ü ���� �޼���
    bool Spawn_Block()
    {
        // Last Block �� ������ ���
        if (lastBlock != null)
            // ���� ��ü�� ��ġ �� ���� (�ֱ� ��ü�� Local ��ġ ��) <- �Ʒ��� ������ �����̹Ƿ� �θ� ��ü ���� ��ǥ�� �����
            prevBlockPosition = lastBlock.localPosition;

        // ���� ������ ��ü ����
        GameObject newBlock = null;
        // ���� ������ ��ü�� Transform ����
        Transform newTrans = null;

        // ���� ������ ��ü �ʱ�ȭ <<- Instantiate (�Ű� ���� ���� �����Ͽ� ��ȯ���ִ� �Լ�)
        newBlock = Instantiate(originBlock);

        // ���� ������ ��ü�� ã�� ������ ���
        if (newBlock == null)
        {
            // ���� �޼��� ���_Object Instantiate Fail
            Debug.LogError("NewBlock Instantiate Failed");
            // ��ȯ_false (�Ʒ� �ڵ� ����)
            return false;
        }

        // �Ű� ������ ���� ���� �޼��� ȣ��
        ColorChange(newBlock);

        // ���� ������ ��ü�� Transform ����
        newTrans = newBlock.transform;
        // �� ��ü�� �ش� Script�� ���� ��ü�� �ڽ� ��ü�� ����
        newTrans.parent = this.transform;
        // �� ��ü�� ��ġ �� ����_���� ��ü�� Vector3.Up(= Y �� +1)
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        // �� ��ü�� ȸ�� �� ����_Quaternion �ʱ� ��(= ȸ���� ���� ����)
        newTrans.localRotation = Quaternion.identity;
        // �� ��ü�� ũ�� �� ����_���� ��ü�� Vector3 �� <- ��ü ũ�� ������ ������ �� �����̱⿡ ���� �߿��� �ڵ�
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        // ������ ��ü�� ���� ����_+1
        stackCount++;

        // ���� ��ġ �� ����_������ Ƚ�� * Y �� -1
        desiredPosition = Vector3.down * stackCount;
        // ���� ��ġ �� �ʱ�ȭ
        blockTransition = 0f;

        // ���� ��ü �� �ʱ�ȭ
        lastBlock = newTrans;

        // �̵� ���� ����_X�� X
        isMovingX = !isMovingX;

        // ��ȯ_true
        return true;
    }

    // ���� ���� ���� �޼���
    Color GetRandomColor()
    {
        // RGB_R �� ����_���� ����_100 ~ 250 / 255
        float r = Random.Range(100f, 250f) / 255f;
        // RGB_G �� ����_���� ����_100 ~ 250 / 255
        float g = Random.Range(100f, 250f) / 255f;
        // RGB_B �� ����_���� ����_100 ~ 250 / 255
        float b = Random.Range(100f, 250f) / 255f;

        // ��ȯ_RGB
        return new Color(r, g, b);
    }

    // �Ű������� ���� ���� �޼���
    void ColorChange(GameObject go)
    {
        // ���� �� ����_�����ϰ�(���� ���� ��, ���� ���� ��, ���� ��ȯ % ��)
        /* 
            �� �ڵ� �ؼ�
            {(stackCount % 11)} : 0 ~ 9����, �� 10���� �ܰ踦 ���� ������ ������ ���ϰ� �� (���� �������, 9�ܰ踦 ���� ���� �������� ���� ��ȭ)
            { / 10f} : 0 ~ 1 ������ ������ ��µǾ�� ��
            ex) 11�ܰ� => 0.0f, 0.1f, 0.2f, 0.3f, ... , 0.9f, 1.0f
            Lerp �ڵ��� 3��° ���� % ��ġ�� �����ϴ� ��������
            ���� ���ӿ����� ������ ������ �����ؾ� ��ȭ�ϴ� ����������
            0 ~ 1 ���� ��, 100%�� ��ȯ�Ǿ�� ��
         */
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 10) / 9f);

        // �Ű� ������ Component_Renderer ����
        Renderer rn = go.GetComponent<Renderer>();

        // �Ű������� ã�� ������ ���
        if (rn == null)
        {
            // ���� �޼��� ���_Renderer Not Found
            Debug.LogError("Renderer is Null");
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }

        // ���� ����(Material)_�����ϰ� ������ ���� ��
        rn.material.color = applyColor;
        // ���� ī�޶� ��� ���� ����_�����ϰ� ������ ���� �� - 10%
        Camera.main.backgroundColor = applyColor - new Color(0.2f, 0.2f, 0.2f);

        // �����ϰ� ������ ���� ���� ���� ���� ���� ������ ���
        if (applyColor.Equals(nextColor) == true)
        {
            // ���� ���� �� ����
            prevColor = nextColor;
            // ���� ���� �� ����_���� ���� �޼��� ȣ��
            nextColor = GetRandomColor();
        }
    }

    // ��ü �̵� �޼���
    void MoveBlock()
    {
        // ���� ��ġ �� ����_60FPS * ��ü �ӷ�
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        // ���� ��ġ ���� �� ����_�ﰢ�Լ� ��� ��(0 ~ ��ü ���� ���̸� ���� ��ġ ����ŭ ����) - (��ü ���� / 2) <- ���� ��ġ�� ���߱� ���� {��ü ���� / 2} ���
        float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

        // �̵� ������ X���� ���
        if (isMovingX)
        {
            // ���� ��ü ��ġ �� ����_Vector3(���� ��ġ ���� �� * ��ü�� �̵��ϴ� ��, ������ ��ü�� ����, �ֱ� ��ü�� ��ġ ��)
            lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition);
        }
        // �̵� ������ X���� �ƴ� ���
        else
        {
            // ���� ��ü ��ġ �� ����_Vector3(�ֱ� ��ü�� ��ġ ��, ������ ��ü�� ����, ���� ��ġ ���� �� * ��ü�� �̵��ϴ� ��)
            lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, -movePosition * MovingBoundsSize);
        }
    }

    // ��ü ���� �޼���
    bool PlaceBlock()
    {
        // ���� ��ġ �� ����
        Vector3 lastPosition = lastBlock.localPosition;

        // �̵� ������ X���� ���
        if (isMovingX)
        {
            // ��� ���� �� ����
            float deltaX = prevBlockPosition.x - lastPosition.x;
            bool isNegativeNum = (deltaX < 0) ? true : false;

            // ���밪 ��ȯ_��� ���� ��
            deltaX = Mathf.Abs(deltaX);
            // ��� ���� ���� �ּ� ������ ���� ���
            if (deltaX > ErrorMargin)
            {
                // ��ü�� ũ�� ����
                stackBounds.x -= deltaX;
                // ��ü�� ũ�Ⱑ 0 ������ ���
                if (stackBounds.x <= 0)
                {
                    // ��ȯ_false
                    return false;
                }
                // �� ��ü ������ �߽� �� ����
                float middle = (prevBlockPosition.x + lastPosition.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaX / 2f;
                CreateRubble(
                    new Vector3(
                        isNegativeNum
                        ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                        : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                        , lastPosition.y
                        , lastPosition.z
                        ),
                    new Vector3(deltaX, 1, stackBounds.y)
                );

                comboCount = 0;
            }
            // ��� ���� ���� �ּ� ������ ���� ���
            else
            {
                // �޺� ���� �޼��� ȣ��
                CheckCombo();
                // ���� ��ü ��ġ �� ����
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }


        }
        // �̵� ������ X���� �ƴ� ���
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0) ? true : false;

            deltaZ = Mathf.Abs(deltaZ);
            if (deltaZ > ErrorMargin)
            {
                stackBounds.y -= deltaZ;
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = (prevBlockPosition.z + lastPosition.z) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaZ / 2f;
                CreateRubble(
                    new Vector3(
                        lastPosition.x,
                        lastPosition.y,
                        isNegativeNum
                        ? lastPosition.z + stackBounds.y / 2 + rubbleHalfScale
                        : lastPosition.z - stackBounds.y / 2 - rubbleHalfScale
                        ),
                    new Vector3(stackBounds.x, 1, deltaZ)
                );

                comboCount = 0;
            }
            else
            {
                CheckCombo();

                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        
        secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    // �޺� ���� �޼���
    void CheckCombo()
    {
        comboCount++;

        if (comboCount > maxCombo)
            maxCombo = comboCount;

        if ( (comboCount % 5) == 0)
        {
            Debug.Log("5 Combo Success!!");
            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x =
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y =
                (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }

    void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("�ְ� ���� ����");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }
}
