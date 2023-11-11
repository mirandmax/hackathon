// Inside BottomTabNavigator.js
import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer } from '@react-navigation/native';
import LeaderboardScreen from './screens/LeaderboardScreen';
import ProfileScreen from './screens/ProfileScreen';
import CameraScreen from './screens/CameraScreen';
import Icon from 'react-native-vector-icons/FontAwesome';

const Tab = createBottomTabNavigator();

const BottomTabNavigator = () => {
  return (
    <NavigationContainer>
      <Tab.Navigator
        screenOptions={{
          style: {
            paddingTop: 5,
          },
        }}
      >
        <Tab.Screen
          name="Profile"
          component={ProfileScreen}
          options={{
            tabBarLabel: 'Profile',
            tabBarIcon: ({ color, size }) => (
              <Icon name="user-circle" color={color} size={size} style={{ marginTop: 5 }} />
            ),
          }}
        />
        <Tab.Screen
          name="Camera"
          component={CameraScreen}
          options={({ route }) => ({
            tabBarLabel: 'Camera',
            tabBarIcon: ({ color, size }) => (
              <Icon name="camera" color={color} size={size} style={{ marginTop: 5 }} />
            ),
            tabBarVisible: route.state && route.state.index === 0, // Hide "Camera" tab if driving
          })}
        />
        <Tab.Screen
          name="Leaderboard"
          component={LeaderboardScreen}
          options={{
            tabBarLabel: 'Leaderboard',
            tabBarIcon: ({ color, size }) => (
              <Icon name="trophy" color={color} size={size} style={{ marginTop: 5 }} />
            ),
          }}
        />
      </Tab.Navigator>
    </NavigationContainer>
  );
};

export default BottomTabNavigator;
