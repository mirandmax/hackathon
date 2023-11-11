// LeaderboardScreen.js
import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, FlatList, Image } from 'react-native';
import axios from 'axios';

const LeaderboardScreen = () => {
  const [leaderboardData, setLeaderboardData] = useState([]);

  async function getAPI() {
    try {
      const response = await fetch('http://172.20.10.2:5266/api/LKW');
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
  
      const data = await response.json();
      setLeaderboardData(data);
    } catch (error) {
      console.error('Error fetching leaderboard data:', error.message);
    }
  }
  

  useEffect(() => {
    getAPI();
  }, []);

  const renderItem = ({ item, index }) => (
    <View
      style={[
        styles.rowContainer,
        { backgroundColor: index === 0 ? 'gold' : index === 1 ? 'silver' : index === 2 ? '#CD7F32' : '#F2F3F5' },
      ]}
    >
      <View style={styles.row}>
        <Text style={styles.rank}>{`${index + 1}.`}</Text>
        <Text style={styles.name}>{item.Name}</Text>
        <Text style={styles.points}>{`Points: ${item.Credits}`}</Text>
      </View>
    </View>
  );
  
  return (
    <View style={styles.container}>
      {/* Image as the title */}
      <Image source={require('../logo.png')} style={styles.logo} />

      <Text style={styles.leaderboardTitle}></Text>

      <FlatList
        data={leaderboardData}
        keyExtractor={(item, index) => index.toString()}
        renderItem={renderItem}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
  },
  leaderboardTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 16,
    textAlign: 'center', // Center the text
  },
  logo: {
    width: 250, // Set the width to take the full width of the screen
    height: 150, // Set the desired height
    resizeMode: 'contain', // Adjust the resizeMode based on your image
    marginBottom: 16,
    left: '10%',
  },
  rowContainer: {
    marginVertical: 8,
    borderRadius: 20,
    overflow: 'hidden',
    backgroundColor: '#fff',
    borderWidth: 1,
    borderColor: '#ddd',
  },
  row: {
    padding: 16,
    borderRadius: 20,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  rank: {
    fontWeight: 'bold',
    fontSize: 16,
  },
  name: {
    flex: 1,
    marginLeft: 8,
    fontSize: 16,
  },
  credits: {
    fontSize: 16,
  },
});

export default LeaderboardScreen;
