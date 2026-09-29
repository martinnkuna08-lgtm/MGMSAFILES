import 'package:flutter/material.dart';

class MitUpView extends StatelessWidget { const MitUpView({super.key});
@override
Widget build(BuildContext context) {
  const fabSize = 56.0;
  return MaterialApp(
    home: Scaffold(
      appBar: AppBar(
        toolbarHeight: 180,
        backgroundColor: Colors.blue,
        leadingWidth: 100,
        leading: Padding(
          padding: const EdgeInsets.all(8.0),
          child: ClipRRect(
            child: Image.asset('assets/images/profile.png', fit: BoxFit.cover),
          ),
        ),
        title: Padding(
          padding: const EdgeInsets.only(left: 16.0, top: 0.0), // Adjust padding as needed
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisSize: MainAxisSize.min,
            children: const [
              Text(
                'Tsundzukani Nkuna',
                style: TextStyle(fontSize: 20),
              ),
              Text(
                'Subtitle or additional text here',
                style: TextStyle(fontSize: 14, color: Colors.white70),
              ),
            ],
          ),
        ),
        centerTitle: false, // Aligns title to the left

        flexibleSpace: SafeArea(
          child: Stack(
            children: [
              // Bottom-left picture boxes
              Padding(
                padding: const EdgeInsets.only(left: 100.0, bottom: 7),
                child: Align(
                  alignment: Alignment.bottomLeft,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: List.generate(5, (i) => GestureDetector(
                      onTap: () {
                        // handle tap
                      },
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(8),
                        child: Container(
                          width: 60,
                          height: 60,
                          color: Colors.white24,
                          child: Image.asset(
                            'assets/images/pic${i + 1}.png',
                            fit: BoxFit.cover,
                          ),
                        ),
                      ),
                    )),
                  ),
                ),
              ),
              // Top-right additional widget
              Positioned(
                child: Padding(
                  padding: const EdgeInsets.only(top: 0, right: 2),
                  child: Align(
                    alignment: Alignment.topRight,
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        TextButton(
                          onPressed: () => print('Mitup clicked'),
                          child: const Text('MitUp', style: TextStyle(color: Colors.white)),
                        ),
                        PopupMenuButton<String>(
                          onSelected: (value) => print('Selected: $value'),
                          itemBuilder: (context) => const [
                            PopupMenuItem(value: 'option1', child: Text('Option 1')),
                            PopupMenuItem(value: 'option2', child: Text('Option 2')),
                          ],
                        ),
                      ],
                    ),
                  ),
                ),
              ),

            ],
          ),
        ),
      ),





      body: Center(child: Text('Welcome to Registration')),
      bottomNavigationBar: BottomNavigationBar(
        type: BottomNavigationBarType.fixed,
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.business), label: 'Work'),
          BottomNavigationBarItem(icon: Icon(Icons.forum), label: 'Messages'),
          BottomNavigationBarItem(icon: Icon(Icons.home), label: 'MitUp'),
          BottomNavigationBarItem(icon: Icon(Icons.call), label: 'Logs'),
          BottomNavigationBarItem(icon: Icon(Icons.local_taxi), label: 'Ride'),
        ],
      ),
      // Overlay input UI
      bottomSheet: SizedBox(


        height: fabSize + 16,

        child: Stack(
          children: [
            // Positioned input row
            Positioned(
              left: 2,
              right: 0 + fabSize + 8,
              top: 15,
              child: TextField(
                decoration: InputDecoration(
                  hintText: 'Write a post...',
                  contentPadding: EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(16),
                  ),

                ),
              ),
            ),
            // Positioned FAB
            Positioned(
              right: 5,
              top: 12,
              child: SizedBox(
                height: fabSize,
                width: fabSize,
                child: FloatingActionButton(
                  onPressed: () => print('Send tapped!'),
                  child: Icon(Icons.send),
                  mini: false,
                ),
              ),
            ),
          ],
        ),
      ),
    ),
  );
}
}