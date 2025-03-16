나만의 유니티 툴로 하나하나 필요하다고 생각했던 기능들을 추가할 예정입니다

1. 빠른 프로젝트 폴더 및 스크립트 생성
   - 대부분의 게임에서 사용하는 매니저 들을 한번에 생성할 수 있도록 제작하였습니다 - CreateInit을통해 템플릿 폴더를 가져올 수 있습니다.
   - 스크립트를 하나하나 생성할때 마다 한번씩 리로드되는 속도가 답답해서 한번에 생성하기 위해 제작하였습니다.
   - TreeView를 활용하여 폴더를 선택하고 그 폴더에 스크립트를 생성할수 있도록 기능을 확장하였습니다
![image](https://github.com/user-attachments/assets/163cf4f3-0bc9-410e-9c28-8e4eed63f6c9)


--- 유용한 어트리뷰트 ---
1. Button 어트리뷰트를 추가하여 ContextMenu와 유사한 방식이지만 인스펙터장에 클릭으로 보여집니다다
![image](https://github.com/user-attachments/assets/bb96e8e2-945d-4947-8293-df9b20003f7b)
![image](https://github.com/user-attachments/assets/7175e584-2400-4441-83a8-547f9ea93aa0)

--- 유용한 툴들 ---
1. 빠른 씬 이동기능
   - 폴더에있는 씬을 하나하나 찾아가며 이동하는것이 불편하여 만들었습니다.
   - Scene/SelectScene을 이용하여 BuildSetting에 등록한 모든 씬들을 빠르게 이동할 수 있습니다.
![image](https://github.com/user-attachments/assets/87622f7d-255a-4d7f-8148-d45269f52bf0)

2. ScriptableObject와 json을 상호 변환해 줍니다
   - 해당 ScriptableObject를 클릭하고 Save해주면 Datas 폴더에 json파일이 생성됩니다.
   - 해당 ScriptableObject를 클릭하고 Load해주면 Datas 폴더에 있던 json파일의 데이터를 불러옵니다.
     
![image](https://github.com/user-attachments/assets/65b7f946-7432-4ae8-b976-715c8db1ab98)


3. 아코디언메뉴 
   - 아코디언 형식의 UI를 바로 생성해 볼수 있습니다.
   - UI/Accordion을 통해 미리 확인할 수 있습니다
   - 의존 패키지로 TextMeshPro를 요구합니다.


