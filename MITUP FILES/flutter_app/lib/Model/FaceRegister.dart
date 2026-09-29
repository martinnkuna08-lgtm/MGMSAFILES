class FaceRegisterModel {
  final int? userId;
  final String faceImagePath; // Path to saved image or base64 string
  FaceRegisterModel({
    this.userId,

    required this.faceImagePath,
  });
}