// import { LinearGradient } from 'expo-linear-gradient'
import { SafeAreaView, StatusBar, Text, TextInput, View, Dimensions, Pressable } from 'react-native'
import React from 'react'

const { width } = Dimensions.get('window')
export default function App() {
  const colors = ['#2F4858', '#4b5563']
  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: '#2F4858' }}>
      <View style={{ flex: 1 }}>
        <View>
          <Text
            style={{ color: '#fff', fontFamily: 'sans-serif', fontWeight: '800', fontSize: 30, alignSelf: 'center' }}
          >
            Create a Budget List
          </Text>
        </View>
      </View>
      <View style={{ flex: 9, backgroundColor: '#fff', borderTopRightRadius: 40, borderTopLeftRadius: 40 }}>
        <View style={{ gap: 30, justifyContent: 'center', alignItems: 'center', margin: 10 }}>
          <View style={{ marginTop: 40 }}>
            <View style={{ marginVertical: 10 }}>
              <Text style={{ fontWeight: 'bold', fontSize: 20 }}>List Name :</Text>
            </View>
            <TextInput
              style={{
                padding: 8,
                backgroundColor: '#ddd',
                fontSize: 20,
                fontWeight: 'bold',
                color: '#fff',
                borderRadius: 10,
                width: width * 0.8,
                height: 50
              }}
              placeholder="Add a budget name"
            />
          </View>
          <View style={{ marginTop: 2 }}>
            <TextInput
              style={{
                padding: 8,
                width: width * 0.8,
                borderRadius: 10,
                color: '#fff',
                fontSize: 20,
                fontWeight: 'bold',
                backgroundColor: '#ddd'
              }}
              multiline={true}
              numberOfLines={10} // Set the number of lines to 4
            />
          </View>

          <View style={{ marginTop: 20, width: width * 0.8 }}>
            <View
              style={{
                borderRadius: 10,
                elevation: 3,
                overflow: 'hidden'
              }}
            >
              <Pressable android_ripple={{ color: '#6D5E85' }} style={{ backgroundColor: '#6D5E91', padding: 10 }}>
                <Text
                  style={{
                    fontSize: 20,
                    fontWeight: 'bold',
                    color: '#fff',
                    textAlign: 'center'
                  }}
                >
                  Save
                </Text>
              </Pressable>
            </View>
          </View>
        </View>
      </View>
      <StatusBar backgroundColor={colors[0]} />
    </SafeAreaView>
  )
}
