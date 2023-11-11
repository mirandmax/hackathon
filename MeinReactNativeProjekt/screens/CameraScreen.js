import React, { useState, useRef } from 'react';
import {
  View,
  Text,
  TouchableOpacity,
  Image,
  TextInput,
  StyleSheet,
  Modal,
  Button,
  Keyboard,
  TouchableWithoutFeedback,
  KeyboardAvoidingView,
} from 'react-native';
import { Camera } from 'expo-camera';
import Icon from 'react-native-vector-icons/FontAwesome';
import axios from 'axios';

const CameraScreen = () => {
  const [hasPermission, setHasPermission] = useState(null);
  const [capturedImage, setCapturedImage] = useState(null);
  const [licenseNumber, setLicenseNumber] = useState('');
  const [email, setEmail] = useState('');
  const [mobileNumber, setMobileNumber] = useState('');
  const [isPopupVisible, setPopupVisible] = useState(false);
  const [earnedPoints, setEarnedPoints] = useState(10);
  const cameraRef = useRef(null);

  const askForCameraPermission = async () => {
    const { status } = await Camera.requestCameraPermissionsAsync();
    setHasPermission(status === 'granted');
  };

  const takePicture = async () => {
    if (cameraRef.current) {
      const { uri } = await cameraRef.current.takePictureAsync();
      setCapturedImage(uri);
    }
  };

  const resetCamera = () => {
    setCapturedImage(null);
    setLicenseNumber('');
    setEmail('');
    setMobileNumber('');
  };

  const submitPhoto = async () => {
    try {
      const postData = {
        LicensePlate: licenseNumber,
        CompanyMail: email,
        CompanyName: "aaa",
        CompanyPhone: mobileNumber,
        Longitude: "12.2", 
        Latitude: "1",     
        UserName: "hallo",  
      };

      const response = await axios.post('http://172.20.10.2:5266/api/LKW', postData);

      const pointsEarned = response.data.earnedPoints || 0;

      setEarnedPoints(pointsEarned);

      resetCamera();

      setPopupVisible(true);
    } catch (error) {
      console.error('Error submitting photo:', error.message);
    }
  };

  const closePopup = () => {
    setPopupVisible(false);
  };

  if (hasPermission === null) {
    askForCameraPermission();
    return <View />;
  }

  if (hasPermission === false) {
    return <Text>No access to camera</Text>;
  }

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={{ flex: 1 }}
    >
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <View style={{ flex: 1 }}>
          {capturedImage ? (
            <Image source={{ uri: capturedImage }} style={styles.capturedImage} />
          ) : (
            <Camera style={{ flex: 1 }} ref={cameraRef}>
              <View style={{ flex: 1, justifyContent: 'flex-end', alignItems: 'center' }}>
                <TouchableOpacity onPress={takePicture} style={styles.cameraButton}>
                  <Icon name="camera" size={50} color="white" />
                </TouchableOpacity>
              </View>
            </Camera>
          )}
          {capturedImage && (
            <View style={styles.formContainer}>
              <View style={styles.formRow}>
                <Text style={styles.label}>License Number:</Text>
                <TextInput
                  style={styles.input}
                  value={licenseNumber}
                  onChangeText={setLicenseNumber}
                />
              </View>
              <View style={styles.formRow}>
                <Text style={styles.label}>E-Mail:</Text>
                <TextInput
                  style={styles.input}
                  value={email}
                  onChangeText={setEmail}
                  keyboardType="email-address"
                />
              </View>
              <View style={styles.formRow}>
                <Text style={styles.label}>Mobile Number:</Text>
                <TextInput
                  style={styles.input}
                  value={mobileNumber}
                  onChangeText={setMobileNumber}
                  keyboardType="phone-pad"
                />
              </View>
            </View>
          )}
          {capturedImage && (
            <View style={styles.buttonContainer}>
              <TouchableOpacity onPress={resetCamera} style={styles.button}>
                <Text style={styles.buttonText}>Return</Text>
              </TouchableOpacity>
              <TouchableOpacity onPress={submitPhoto} style={styles.button}>
                <Text style={styles.buttonText}>Submit</Text>
              </TouchableOpacity>
            </View>
          )}

          {/* Popup */}
          <Modal
            animationType="slide"
            transparent={true}
            visible={isPopupVisible}
            onRequestClose={closePopup}
          >
            <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
              <View style={{ backgroundColor: 'white', padding: 20, borderRadius: 10, elevation: 5 }}>
                <Text>Photo submitted! You earned 10 points.</Text>
                <Button title="Close" onPress={closePopup} />
              </View>
            </View>
          </Modal>
        </View>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  capturedImage: {
    flex: 1,
    resizeMode: 'cover',
  },
  formContainer: {
    backgroundColor: 'white',
    padding: 10,
    borderRadius: 10,
    elevation: 5,
  },
  formRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 10,
  },
  label: {
    fontWeight: 'bold',
    marginRight: 10,
  },
  input: {
    flex: 1,
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 5,
    paddingHorizontal: 10,
  },
  cameraButton: {
    marginBottom: 20,
    backgroundColor: 'transparent',
  },
  buttonContainer: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    padding: 20,
  },
  button: {
    backgroundColor: 'blue',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
  },
  buttonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});

export default CameraScreen;