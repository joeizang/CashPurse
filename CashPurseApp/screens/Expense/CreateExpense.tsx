import React from 'react'
import { Dimensions, Pressable, ScrollView, Text, TextInput, View } from 'react-native'

const { width, height } = Dimensions.get('window')

export default function CreateExpense() {
  return (
    <View style={{ flex: 10, height: height }}>
      <View
        style={{
          flex: 9,
          backgroundColor: '#A36198',
          borderTopRightRadius: 40,
          borderTopLeftRadius: 40,
          marginTop: 40
        }}
      >
        <View style={{ marginTop: 10 }}>
          <View>
            <Text
              style={{
                color: '#fff',
                fontFamily: 'sans-serif',
                fontWeight: '800',
                fontSize: 25,
                alignSelf: 'center'
              }}
            >
              Create expense
            </Text>
          </View>
        </View>
        <ScrollView
          style={{
            gap: 30,
            borderTopRightRadius: 40,
            borderTopLeftRadius: 40,
            backgroundColor: '#fff',
            marginTop: 30,
            width: width,
            height: '100%'
          }}
          contentContainerStyle={{ alignItems: 'center' }}
        >
          <View style={{ marginTop: 1 }}>
            <View style={{ marginVertical: 10, marginTop: 40 }}>
              <Text style={{ fontWeight: 'bold', fontSize: 20, color: '#333' }}>List Name :</Text>
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
                height: 50,
                textAlign: 'left'
              }}
              placeholder="Add a budget name"
            />
          </View>
          <View style={{ marginTop: 2 }}>
            <View style={{ marginVertical: 10 }}>
              <Text style={{ fontWeight: 'bold', fontSize: 20 }}>Description :</Text>
            </View>
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
              placeholder="Add a description"
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
        </ScrollView>
      </View>
    </View>
  )
}
