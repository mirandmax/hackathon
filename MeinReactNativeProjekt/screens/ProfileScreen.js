// ProfileScreen.js
import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, TouchableWithoutFeedback, Keyboard, Alert, Image, StyleSheet } from 'react-native';
import axios from 'axios';

const ProfileScreen = ({ navigation }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [isSignUp, setIsSignUp] = useState(true);
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const handleSignUp = async () => {
    if (!username || !password || !repeatPassword) {
      Alert.alert('Incomplete Form', 'Please fill in all fields.');
      return;
    }

    if (password !== repeatPassword) {
      Alert.alert('Password Mismatch', 'Password and Repeat Password do not match.');
      return;
    }

    try {
      const postData = {
        Name: username,
        Password: password,
      };

      const response = await axios.post('http://172.20.10.2:5266/api/user/signup', postData);

      if (response.status === 200) {
        Alert.alert('Sign Up Successful', 'You are now logged in');
        setIsSignUp(false);
        setIsLoggedIn(true);
        resetFields();
      } else {
        Alert.alert('Sign Up Failed', 'An error occurred during sign-up. Please try again.');
      }
    } catch (error) {
      console.error('Sign Up Error:', error);
      Alert.alert('Error', 'An error occurred during sign-up. Please try again.');
    }
  };

  const handleLogin = async () => {
    if (!username || !password) {
      Alert.alert('Incomplete Form', 'Please fill in all fields.');
      return;
    }
  
    try {
      const repData = {
        Name: username,
        Password: password,
      };
  
      const response = await axios.post("http://172.20.10.2:5266/api/user/login", repData);
  
      if (response.status === 200) {
        Alert.alert('Login Successful', 'Welcome back!');
        setIsLoggedIn(true);
        navigation.navigate('Camera');
      } else {
        Alert.alert('Login Failed', 'Invalid username or password. Please try again.');
      }
    } catch (error) {
      console.error('Login Error:', error);
      Alert.alert('Error', 'An error occurred during login. Please try again.');
    }
  };
  

  const handleLogout = () => {
    setIsLoggedIn(false);
    setUsername('');
    setPassword('');
    setRepeatPassword('');
  };

  const resetFields = () => {
    setPassword('');
    setRepeatPassword('');
  };

  const handleToggle = () => {
    setIsSignUp(!isSignUp);
    resetFields();
    Keyboard.dismiss();
  };

  return (
    <TouchableWithoutFeedback onPress={Keyboard.dismiss} accessible={false}>
      <View style={styles.container}>
        {/* Logo View */}
        <View style={styles.logoContainer}>
          <Image source={require('../logo.png')} style={styles.logo} resizeMode="contain" />
        </View>

        {isLoggedIn ? (
          <View style={styles.contentContainer}>
            <Image source={require('../fake_profile_picture.png')} style={styles.profilePicture} />
            <Text style={styles.username}>{username}</Text>
            <TouchableOpacity style={styles.logoutButton} onPress={handleLogout}>
              <Text style={styles.buttonText}>Logout</Text>
            </TouchableOpacity>
          </View>
        ) : (
          <View style={styles.contentContainer}>
            <Text style={styles.title}>{isSignUp ? 'Sign Up' : 'Login'}</Text>
            <TextInput
              style={styles.input}
              placeholder="Username"
              value={username}
              onChangeText={setUsername}
            />
            <TextInput
              style={styles.input}
              placeholder="Password"
              secureTextEntry
              value={password}
              onChangeText={setPassword}
            />
            {isSignUp && (
              <TextInput
                style={styles.input}
                placeholder="Repeat Password"
                secureTextEntry
                value={repeatPassword}
                onChangeText={setRepeatPassword}
              />
            )}
            <TouchableOpacity
              style={styles.button}
              onPress={isSignUp ? handleSignUp : () => { handleLogin(); }}
            >
              <Text style={styles.buttonText}>{isSignUp ? 'Sign Up' : 'Login'}</Text>
            </TouchableOpacity>
            <TouchableOpacity onPress={handleToggle}>
              <Text style={styles.toggleText}>
                {isSignUp ? 'Already have an account? Login' : "Don't have an account? Sign Up"}
              </Text>
            </TouchableOpacity>
          </View>
        )}
      </View>
    </TouchableWithoutFeedback>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  logoContainer: {
    alignItems: 'center',
    marginBottom: 20,
  },
  logo: {
    width: 120,
    height: 40,
  },
  contentContainer: {
    // Add styles as needed for the content container
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center', // Center text horizontally
  },
  input: {
    width: 300,
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    borderRadius: 5,
    marginBottom: 10,
    paddingHorizontal: 10,
  },
  button: {
    backgroundColor: 'blue',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
    marginBottom: 10,
  },
  buttonText: {
    color: 'white',
    fontWeight: 'bold',
    textAlign: 'center', // Center text horizontally
  },
  toggleText: {
    color: 'blue',
    textAlign: 'center', // Center text horizontally
  },
  profilePicture: {
    width: 120,
    height: 120,
    borderRadius: 60,
    marginBottom: 20,
  },
  username: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
  },
  logoutButton: {
    backgroundColor: 'red',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
    marginBottom: 10,
  },
  logoContainer:{
    alignItems: 'center',
    marginBottom: 40,
  },
  logo: {
    width: 250,
    height: 150,
  },
});

export default ProfileScreen;
