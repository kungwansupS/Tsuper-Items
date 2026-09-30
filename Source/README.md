# Source code สำหรับศึกษา

ไฟล์ `.cs` ทั้ง 8 ไฟล์คัดจาก Tsuper Items 0.6.2 โดยไม่แก้เนื้อหาโค้ด
จัดโฟลเดอร์ตามต้นทางและคง namespace `SuperItem` เพื่อให้อ่านความสัมพันธ์ได้
นำไปศึกษาและดัดแปลงเพื่อพัฒนาต่อได้ตาม [ประกาศสิทธิ์](../COPYRIGHT.md)
งาน art ทั้งหมดยังคงเป็นสิทธิ์ของผู้สร้างแต่ละราย

## ไฟล์ที่รวมไว้

| ไฟล์ | สิ่งที่ใช้ศึกษา |
| --- | --- |
| [Items/ExcaliburZ.cs](Items/ExcaliburZ.cs) | `SetDefaults` ของอาวุธและการลงทะเบียนสูตรคราฟต์ด้วย `AddRecipes` |
| [Items/SpectacleWeapon.cs](Items/SpectacleWeapon.cs) | คลาสฐานอาวุธ การกำหนด projectile ทิศเล็ง ท่าใช้ และเงื่อนไขให้ผู้เล่นเจ้าของอาวุธสร้าง projectile |
| [Common/WeaponKind.cs](Common/WeaponKind.cs) | แยกชนิดอาวุธและค่าคูลดาวน์ ความเร็ว และตัวคูณความเสียหาย รวมถึงจำกัดผลคูณให้อยู่ในช่วง `int` |
| [Common/SwordMotion.cs](Common/SwordMotion.cs) | คำนวณท่าดาบจาก progress และช่วงที่การโจมตี active |
| [Common/AttackGeometry.cs](Common/AttackGeometry.cs) | คำนวณจุดบนรูปโค้งที่ใช้ร่วมกันระหว่างการวาดและการชน |
| [Visuals/Easing.cs](Visuals/Easing.cs) | easing, envelope และ flash สำหรับเปลี่ยนค่าตามช่วงเวลา |
| [Common/SuperItemConfig.cs](Common/SuperItemConfig.cs) | แยก config ฝั่ง server/client และกำหนด default กับช่วงค่าที่ปรับได้ |
| [Visuals/ScreenShake.cs](Visuals/ScreenShake.cs) | กล้องสั่นที่ลดแรงตามเวลา ใช้ config ฝั่ง client และหยุดขยับกล้องเมื่อ pause |

## เริ่มอ่านอย่างไร

เริ่มจาก `ExcaliburZ` เพื่อดูค่าของอาวุธและสูตร จากนั้นอ่าน `SpectacleWeapon`
เพื่อดูพฤติกรรมที่ใช้ร่วมกัน และ `WeaponRules` ใน `WeaponKind.cs` เพื่อดูการแยกค่าของแต่ละชนิด
ส่วน `SwordMotion`, `AttackGeometry` และ `Easing` แสดงวิธีคำนวณตำแหน่งและจังหวะด้วยคณิตศาสตร์
อ่าน `SuperItemConfig` คู่กับ `ScreenShake` เพื่อดูการเชื่อมค่าตั้งค่ากับเอฟเฟกต์ฝั่งผู้เล่น

## Dependency และขอบเขต

ชุดนี้เป็น **ซอร์สบางส่วน** ไม่ใช่โปรเจกต์ที่ build Tsuper Items ทั้งม็อดได้
ต้นทางใช้ .NET 8, C# 12 และเปิด `ImplicitUsings` จึงมีการใช้ `Math`/`MathF`
โดยไม่ได้เขียน `using System;` ในทุกไฟล์ คลาส `internal` ต้องอยู่ใน assembly เดียวกัน
หรือปรับการเข้าถึงให้เหมาะกับโครงการของตน

- `WeaponKind` และ `Easing` ใช้ API ของ .NET
- `AttackGeometry` และ `SwordMotion` ใช้ `Microsoft.Xna.Framework` จาก FNA ที่มากับ tModLoader; `SwordMotion` ใช้ `WeaponKind` ด้วย
- `SuperItemConfig` และ `ScreenShake` ใช้ Terraria / tModLoader; `ScreenShake` ใช้ `SuperItemClientConfig` จากไฟล์ config ที่รวมไว้
- `ExcaliburZ` ใช้ `SpectacleWeapon` ซึ่งรวมไว้ แต่คลาสฐานยังเรียก `SpectralBolt`, `HeldSword`, `SuperItemPlayer` และ `VfxDraw` ที่ไม่ได้รวมในชุดนี้

เมื่อประยุกต์ใช้ ให้ปรับ namespace และชนิดอาวุธให้เข้ากับม็อดของตน
เขียนหรือแทนที่ dependency ที่ไม่ได้รวม และจัดทำ texture กับ localization ของตนเอง
ค่า grip/tip ใน `SwordMotion` อ้างอิงขนาดภาพดาบต้นทาง จึงควรปรับให้ตรงกับงาน art ใหม่
ค่า damage และคูลดาวน์ในตัวอย่างเป็นค่าของ Tsuper Items ไม่ใช่คำแนะนำด้านสมดุลสำหรับทุกม็อด

การตรวจชุดนี้คอมไพล์ไฟล์ช่วยทั้ง 6 ไฟล์กับ tModLoader ที่ติดตั้ง
และตรวจไฟล์อาวุธทั้ง 2 ไฟล์ในบริบทซอร์สต้นทาง ดูบันทึกการตรวจด้านล่าง

## เอกสารของ tModLoader

- [Basic Item](https://github.com/tModLoader/tModLoader/wiki/Basic-Item) สำหรับ `ModItem`, `SetDefaults` และสูตรคราฟต์
- [Basic JSON & ModConfigs](https://github.com/tModLoader/tModLoader/wiki/Basic-JSON-%26-ModConfigs) สำหรับ config ฝั่ง client/server

## บันทึกการตรวจ

วันที่ 2026-09-30: ตรวจ SHA256 ของไฟล์ `.cs` ทั้ง 8 ไฟล์ให้ตรงกับซอร์สต้นทาง
คอมไพล์ไฟล์ช่วยทั้ง 6 ไฟล์ และไฟล์อาวุธทั้ง 2 ไฟล์ร่วมกับ dependency จากต้นทาง
ด้วย tModLoader 2026.7.3.0 / .NET SDK 10.0.400 โดย target .NET 8 และ C# 12
ไม่มีการทดสอบในเกมรอบใหม่สำหรับชุดตัวอย่างนี้
