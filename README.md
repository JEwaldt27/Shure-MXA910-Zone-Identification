The speaker tracking camera system cost a fortune, so I set down the road to reverse engineer how they work. This is just the start for this project!

How it works:
Step 1: Connect to Shure MXA910 (may work with other Shure mic, but MXA910 confirmed)
Step 2: Read the output strings
Step 3: Parse string for lobe activation and deactivation
Step 4: Set variables to track active lobe
Step 5: Track average amount of time each lobe is active
*Future plans moving forward*
Step 6: If a lobe is active for a set amount of time, send command with active lobe to crestron control system
Step 7: Crestron control system will receive the active lobe and send a command to the Vaddio camera to recall a preset. (Set preset up manually before)
Step 8 (Optional): Create a flow within crestron that will allow for diabling of speaker tracking at will from the touch panel in confrence room
