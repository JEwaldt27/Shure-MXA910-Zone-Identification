# Speaker Tracking Camera System Reverse Engineering

Commercial speaker-tracking camera systems are often prohibitively expensive. This project aims to reverse engineer their operation to create a cost-effective alternative. Below is an overview of the current approach and future development plans.

---

## How It Works

1. **Connect to Shure MXA910**  
   (Confirmed with MXA910; may also work with other Shure microphones)

2. **Read** the microphone’s output strings

3. **Parse** the output to detect lobe activation and deactivation

4. **Track** the currently active lobe using variables

5. **Calculate** the average duration each lobe remains active

---

## *Future Development Plans*

6. If a lobe remains active beyond a configurable threshold (to avoid unnecessary camera movements from brief noises), **send a command** with the active lobe information to the Crestron control system

7. The Crestron system will **process the active lobe data** and issue a command to the Vaddio camera to recall the appropriate preset *(presets must be manually configured in advance)*

8. *(Optional)* Develop a control flow within the Crestron system to **enable or disable** speaker tracking from the conference room’s touch panel
