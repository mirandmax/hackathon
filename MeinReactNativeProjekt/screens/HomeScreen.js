// HomeScreen.js
import React, { useEffect, useState } from 'react';
import { View, Text, Button } from 'react-native';
import Geolocation from 'react-native-geolocation-service';
import axios from 'axios';

const HomeScreen = ({ navigation }) => {
  const [isDriving, setIsDriving] = useState(false);

  useEffect(() => {
    const watchId = Geolocation.watchPosition(
      position => {
        const { latitude, longitude, speed } = position.coords;
        // Perform logic to determine if the user is driving based on speed
        const drivingStatus = checkDrivingStatus(speed);
        setIsDriving(drivingStatus);
      },
      error => console.log('Error getting location:', error),
      {
        enableHighAccuracy: true,
        distanceFilter: 10, // Minimum distance (in meters) to trigger onLocationChanged
        fastestInterval: 1000, // Minimum time (in milliseconds) between updates
      },
    );

    return () => {
      Geolocation.clearWatch(watchId);
    };
  }, []);

  const checkDrivingStatus = speed => {
    // Customize this logic based on your requirements
    return speed > 5; // Assume the user is driving if speed is greater than 5 m/s
  };

  return (
    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
      <Text>Home Screen</Text>
      {isDriving ? (
        <Text>You are currently driving. App is locked.</Text>
      ) : (
        <Button title="Go to Profile" onPress={() => navigation.navigate('Profile')} />
      )}
    </View>
  );
};

export default HomeScreen;
