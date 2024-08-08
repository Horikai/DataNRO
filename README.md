# DataNRO
Xem dữ liệu vật phẩm, quái, NPC, map,... của game [**Ngọc Rồng Online**](http://ngocrongonline.com/) và [**Hồi sinh Ngọc Rồng**](https://hoisinhngocrong.com/). Dữ liệu được tự động cập nhật mỗi ngày.
<br>*Mã nguồn của phần lấy dữ liệu từ game **Hồi sinh Ngọc Rồng** là **độc quyền** và **không có trong kho mã nguồn này**.*
## API
Định dạng: `Loại Game`/`Server`/`Loại dữ liệu`
### Loại Game / Server
- `/TeaMobi`: Data game Ngọc Rồng Online
  + `/Server`*: Server 1-7 và 11-13
  + `/Server8910`: Server gộp (server 8, 9, 10)
  + `/Super`*: Server Super 1 và Super 2
  + `/Universe1`: Server Quốc tế (Universe 1)
  + `/Naga`: Server Indonesia (Naga)
- `/HSNR`: Data game Hồi sinh Ngọc Rồng
  + `/Server`*: Server 1-3
### Loại dữ liệu
- `ItemOptionTemplates.json`: Dữ liệu loại thuộc tính vật phẩm
```json
  {
    "id": 93,
    "type": 0,
    "name": "HSD # ngày"
  }
```
- `ItemTemplates.json`: Dữ liệu vật phẩm, chứa tên, mô tả, hành tinh, sức mạnh yêu cầu,...
```json
  {
    "isUpToUp": false,
    "id": 26,
    "type": 2,
    "gender": 2,
    "level": 3,
    "strRequire": 50000,
    "iconID": 410,
    "part": -1,
    "name": "Găng sắt",
    "description": "Giúp tăng sức đánh"
  }
```
- `LastUpdated`: Thời gian mới nhất dữ liệu được cập nhật
```
2024-08-07T09:42:37.0621765Z
```
- `Maps.json`: Dữ liệu map
```json
  {
    "id": 155,
    "name": "Nhà tù Hồi sinh ngọc rồng"
  }
```
- `MobTemplates.json`: Dữ liệu quái
```json
  {
    "mobTemplateId": 80,
    "rangeMove": 0,
    "speed": 1,
    "type": 0,
    "dartType": 25,
    "hp": 2000000000,
    "name": "Mộc Nhân Super"
  }
```
- `NClasses.json`: Dữ liệu class nhân vật
```json
{
    "classId": 0,
    "name": "Trái đất",
    "skillTemplates": [
        {
        "id": 0,
        "maxPoint": 7,
        "manaUseType": 0,
        "type": 1,
        "iconId": 539,
        "name": "Chiêu đấm Dragon",
        "description": "Tấn công cận chiến",
        "damInfo": "Tăng sức đánh: #%",
        "skills": [
          {
            "point": 1,
            "maxFight": 1,
            "manaUse": 1,
            "skillId": 0,
            "dx": 32,
            "dy": 18,
            "damage": 100,
            "price": 0,
            "coolDown": 500,
            "powRequire": 1000,
            "moreInfo": "tại ông nội ngay lúc đầu"
          },
          ...
        ]
      },
      ...
    ]
}
```
- `NpcTemplates.json`: Dữ liệu NPC
```json
  {
    "npcTemplateId": 70,
    "headId": 1968,
    "bodyId": 1969,
    "legId": 1970,
    "name": "Mafia Internet <D2F>",
    "menu": [
      ["Nói chuyện"]
    ]
  }
```